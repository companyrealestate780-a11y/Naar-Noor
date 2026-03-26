import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';

/**
 * Example component template for Naar & Noor project
 * 
 * This template follows the project's conventions:
 * - Standalone component (no NgModule)
 * - CUSTOM_ELEMENTS_SCHEMA for iconify icons
 * - CommonModule for common directives
 */

@Component({
  selector: 'app-example-section',
  standalone: true,
  imports: [CommonModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './example-section.component.html',
  styleUrls: ['./example-section.component.css']
})
export class ExampleSectionComponent {
  // Component properties
  title = 'Section Title';
  items: string[] = [];

  // Lifecycle hooks
  ngOnInit() {
    // Initialization logic
  }

  // Component methods
  handleClick() {
    console.log('Button clicked');
  }
}
