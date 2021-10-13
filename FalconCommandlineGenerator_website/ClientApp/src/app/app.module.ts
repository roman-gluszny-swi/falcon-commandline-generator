import { BrowserModule } from '@angular/platform-browser';
import { NgModule, TRANSLATIONS, MissingTranslationStrategy, TRANSLATIONS_FORMAT } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { CatalogItemsComponent } from './catalog-items/catalog-items.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommandlineComponent } from './commandline/commandline.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NuiButtonModule, NuiCheckboxModule, NuiBusyModule, NuiSelectV2Module, NuiTextboxModule, NuiTableModule } from "@solarwinds/nova-bits";


@NgModule({
  declarations: [
    AppComponent,
    CatalogItemsComponent,
    CommandlineComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NuiButtonModule,
    NuiCheckboxModule,
    NuiBusyModule,
    NuiSelectV2Module,
    NuiTextboxModule,
    NuiTableModule
  ],
  providers: [{provide: TRANSLATIONS_FORMAT, useValue: "xlf"},{provide: TRANSLATIONS, useValue: ""}, AppComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
