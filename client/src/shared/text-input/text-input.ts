import { Component, input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  imports: [ReactiveFormsModule],
  templateUrl: './text-input.html',
  styleUrl: './text-input.css',
})
export class TextInput implements ControlValueAccessor {

  // ControlValueAccessorot ke kaze na Angular deka ovaa komponenta e FormControl

  label = input<string>('')
  type = input<string>('text')

  maxDate = input<string>('')
  
  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this
  }

  writeValue(obj: any): void {

  }

  registerOnChange(fn: any): void {

  }

  registerOnTouched(fn: any): void {

  }

  // koga ima get keyword vo template-ot funk. ja pristpuvas kako promenliva
  get control(): FormControl {
    return this.ngControl.control as FormControl
  }

  
}
