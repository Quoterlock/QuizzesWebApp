import React, { useState } from 'react';

interface Props {
    onLoad:(bytes:Uint8Array) => void,
}


export function FileUpload({onLoad}:Props) {
  const [fileBytes, setFileBytes] = useState<Uint8Array | null>(null);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      const reader = new FileReader();

      reader.onload = () => {
        const arrayBuffer = reader.result as ArrayBuffer;
        const byteArray = new Uint8Array(arrayBuffer);
        setFileBytes(byteArray);
        onLoad(byteArray)

        console.log(byteArray); // Log byte[] to the console or handle it as needed
      };

      reader.onerror = () => {
        console.error('Error reading file');
      };

      reader.readAsArrayBuffer(file);
    }
  };

  return (
    <div>
      <input type="file" onChange={handleFileChange} />
      {fileBytes && (
        <div>
          <p>File loaded with {fileBytes.length} bytes.</p>
          {/* You can add further handling or display here */}
        </div>
      )}
    </div>
  );
};

export default FileUpload;
