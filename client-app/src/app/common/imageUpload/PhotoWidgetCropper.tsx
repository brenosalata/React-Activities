import React, { useState } from 'react';
import { Cropper } from 'react-cropper';

interface Props{
    imagePreview: string;
    setCropper: (cropper: Cropper) => void;
}

export default function PhotoWidget({imagePreview, setCropper} : Props){
    const [crop, setCrop] = useState({ x: 0, y: 0 })
    const [zoom, setZoom] = useState(1)
    return(
        // <Cropper
        // src={imagePreview}
        // style={{height: 200, width: '100%'}}
        // initialAspectRatio={1}
        // aspectRatio={1}
        // preview='.img-preview'
        // guides={false}
        // viewMode={1}
        // autoCropArea={1}
        // background={false}
        // onInitialized={cropper => setCropper(cropper)}
        // />
        <div className='cropper'> 
            <Cropper
                src={imagePreview}
                style={{height: 200, width: '100%'}}
        initialAspectRatio={1}
        aspectRatio={1}
        preview='.img-preview'
        guides={false}
        viewMode={1}
        autoCropArea={1}
        background={false}
        onInitialized={cropper => setCropper(cropper)}
            />
        </div>
    )
}