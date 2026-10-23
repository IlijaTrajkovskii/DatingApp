import { Component, input, output, signal } from '@angular/core';

@Component({
  selector: 'app-image-upload',
  imports: [],
  templateUrl: './image-upload.html',
  styleUrl: './image-upload.css',
})
export class ImageUpload {

  protected imageSrcPreview = signal< string | ArrayBuffer | null | undefined>(null);
  protected isDragging = false;
  private fileToUpload: File | null = null;

  uploadFile = output<File>();
  loading = input<boolean>(false);

  onDragOver(event: DragEvent)
  {
    event.preventDefault()
    event.stopPropagation()
    this.isDragging = true
  }

  onDragLeave(event: DragEvent)
  {
    event.preventDefault()
    event.stopPropagation()
    this.isDragging = false
  }

  onDrop(event: DragEvent)
  {
    event.preventDefault()
    event.stopPropagation()
    this.isDragging = false

    if(event.dataTransfer?.files.length)
    {
      const file = event.dataTransfer.files[0] // we allow 1 image at a time to upload
      this.previewImage(file)
      this.fileToUpload = file
    }

  }

  onCancel()
  {
    this.fileToUpload = null
    this.imageSrcPreview.set(null)
  }

  onUpload()
  {
    if(this.fileToUpload)
    {
      this.uploadFile.emit(this.fileToUpload)
    }
  }

  private previewImage(file: File)
  {
    const reader = new FileReader()
    reader.onload = (e) => this.imageSrcPreview.set(e.target?.result)   // go stavame imageSrc 
    reader.readAsDataURL(file)

  }

  

}
