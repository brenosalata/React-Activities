using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class Update
    {
        public class Command : IRequest<Result<Photo>>
        {
            public string Id { get; set; }
        }


        public class Handler : IRequestHandler<Command, Result<Photo>>{
            private readonly DataContext _context;
            private readonly IPhotoAcessor _photoAcessor;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IPhotoAcessor photoAcessor, IUserAccessor userAccessor)
            {
                _context = context;
                _photoAcessor = photoAcessor;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Photo>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

                if(user == null) return null;

                var photoUpdate = user.Photos.FirstOrDefault(x => x.Id == request.Id);
                var photoMain = user.Photos.FirstOrDefault(x => x.IsMain == true);

                if (photoUpdate != null)
                {
                    if(photoMain != null)
                    {
                        if(photoMain.Id == photoUpdate.Id)
                        {
                            user.Photos.Remove(photoUpdate);
                            photoUpdate.IsMain = !photoUpdate.IsMain;
                            user.Photos.Add(photoUpdate);
                        }
                        if(photoMain.Id != photoUpdate.Id)
                        {
                            user.Photos.Remove(photoMain);
                            photoMain.IsMain = !photoMain.IsMain;
                            user.Photos.Add(photoMain);

                            user.Photos.Remove(photoUpdate);
                            photoUpdate.IsMain = !photoUpdate.IsMain;
                            user.Photos.Add(photoUpdate);
                        }
                    }else
                    {
                        user.Photos.Remove(photoUpdate);
                        photoUpdate.IsMain = !photoUpdate.IsMain;
                        user.Photos.Add(photoUpdate);
                    }
                    

                    var result = await _context.SaveChangesAsync() > 0;

                    if (result) return Result<Photo>.Success(photoUpdate);
                }

                return Result<Photo>.Failure("Problem updating photo");
            }
        }
    }
}