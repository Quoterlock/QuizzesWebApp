import React, { useEffect, useState } from 'react';

interface ImageFileProps {
    file?: File,
    height:number,
    width:number
}

const ImageFile: React.FC<ImageFileProps> = ({ file, height, width }) => {
  const [imageSrc, setImageSrc] = useState<string | null>(null);

  useEffect(() => {
    // Function to read the file and convert it to a data URL
    const readFile = (file: Blob) => {
      const reader = new FileReader();

      reader.onload = () => {
        setImageSrc(reader.result as string); // Set the data URL as the image source
      };

      reader.onerror = () => {
        console.error("Error reading file");
        setImageSrc(null);
      };

      // Read the file as a data URL
      reader.readAsDataURL(file);
    };

    if (file instanceof File) {
      file.arrayBuffer().then((r) => readFile(new Blob([r], {type: "image/jpeg"})))
    }
  }, [file]);

  return (
    <div className='mx-auto mb-3'>
      {imageSrc ? (
        <img className='profile-photo' src={imageSrc} alt="Uploaded" style={{ maxWidth: `${width}px`, maxHeight: `${height}px` }} />
      ) : (
        <p>No image to display</p>
      )}
    </div>
  );
};

export default ImageFile;