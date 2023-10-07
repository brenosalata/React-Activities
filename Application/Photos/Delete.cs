using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain;
using Persistence;

namespace Application.Photos
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>{
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IPhotoAcessor _photoAcessor;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IPhotoAcessor photoAcessor, IUserAccessor userAccessor)
            {
                _context = context;
                _photoAcessor = photoAcessor;
                _userAccessor = userAccessor;

            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = _context.Users.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName()).Result;

                if (user == null) return null;

                var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

                if(photo == null) return null;

                if (photo.IsMain) return Result<Unit>.Failure("You cannot delete your main photo");

                var result = await _photoAcessor.DeletePhoto(photo.Id);

                if (result == null) return Result<Unit>.Failure("Problem deleting photo from Cloudinary");

                user.Photos.Remove(photo);

                var sucess = await _context.SaveChangesAsync() > 0;

                if(sucess) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Problem deleting photo from API");
            }
        }
    }
}