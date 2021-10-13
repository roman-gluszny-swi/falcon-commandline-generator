import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommandlineGenerator } from './commandlineGenerator';
import { CatalogItem } from './catalogItem';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommandlineGeneratorService {
  http: HttpClient;
  baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  generateCommandline(commandlineGenerator: CommandlineGenerator): Observable<string[]> {
    return (this.http.post<string[]>(this.baseUrl + 'api/CommandlineGenerator/Generate', commandlineGenerator));
  }

  copyToClipboard(commandlineGenerator: CommandlineGenerator): void {
    this.http.post(this.baseUrl + 'api/CommandlineGenerator/Copy', commandlineGenerator)
      .subscribe(result => {
      }, error => console.error(error));
  }

  runInstaller(commandlineGenerator: CommandlineGenerator) {
    this.http.post(this.baseUrl + 'api/CommandlineGenerator/Install', commandlineGenerator)
      .subscribe(result => {
      }, error => console.error(error));
  }


  //possibly extract into new service
  getBuildNumbers(catalogItem: CatalogItem, itemType: string): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl +
      'api/ProductCatalogHelper/BuildNumbers/' +
      catalogItem.name +
      "/" +
      catalogItem.catalogStage +
      "/" +
      catalogItem.productStage +
      "/" +
      catalogItem.versionNumber +
      "/" +
      itemType);
  }

  getVersionNumbers(catalogItem: CatalogItem, itemType: string): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl +
      'api/ProductCatalogHelper/VersionNumbers/' +
      catalogItem.name +
      "/" +
      catalogItem.catalogStage +
      "/" +
      catalogItem.productStage +
      "/" +
      itemType);
  }

  getNote(catalogItem: CatalogItem, itemType: string): Observable<string> {
    return this.http.get<string>(this.baseUrl +
      'api/ProductCatalogHelper/Note/' +
      catalogItem.name +
      "/" +
      catalogItem.catalogStage +
      "/" +
      catalogItem.productStage +
      "/" +
      catalogItem.buildNumber +
      "/" +
      itemType);
  }

  getItemNames(itemType: string) {
    const url = "api/ProductCatalogHelper/All" + itemType.charAt(0).toUpperCase() + itemType.slice(1, -1) + "Names";
    return this.http.get<string[]>(this.baseUrl + url)
  }
}
