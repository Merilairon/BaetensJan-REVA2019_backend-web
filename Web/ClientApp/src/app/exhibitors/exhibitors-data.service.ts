import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {Exhibitor} from "../models/exhibitor.model";
import {Category} from "../models/category.model";
import {Question} from "../models/question.model";

@Injectable({
  providedIn: 'root'
})
export class ExhibitorsDataService {
  /**
   * Base url of the to connected connection
   */
  private readonly _appUrl = '/API/Exhibitor';

  /**
   * Constructor
   *
   * @param http
   */
  constructor(private http: HttpClient) {
  }

  /**
   * Makes call to backend and returns all exhibitors
   */
  get exhibitors(): Observable<Exhibitor[]> {
    return this.http
      .get(`${this._appUrl}/Exhibitors/`)
      .pipe(map((list: any[]): Exhibitor[] => list.map(Exhibitor.fromJSON)));
  }

  /**
   * Makes call to backend and returns Exhibitor
   */
  exhibitorByName(exhibitorName: string): Observable<Exhibitor> {
    return this.http
      .get(`${this._appUrl}/ExhibitorByName/${exhibitorName}`)
      .pipe(map(Exhibitor.fromJSON));
  }

  /**
   * Makes call to the backend and removes an exhibitor
   *
   * @param rec
   */
  verwijderExhibitor(rec: Exhibitor): Observable<Exhibitor> {
    return this.http
      .delete(`${this._appUrl}/RemoveExhibitor/${rec.id}`)
      .pipe(map(Exhibitor.fromJSON));
  }


  /**
   * Makes call to the backend and removes all the exhibitors
   * @param rec
   */
  removeExhibitors(): Observable<Exhibitor[]> {
    return this.http
      .delete(`${this._appUrl}/RemoveExhibitors`)
      .pipe(map((list: any[]): Exhibitor[] => list.map(Exhibitor.fromJSON)));
  }


  /**
   * Makes call to backend and adds an exhibitor
   *
   * @param exhibitor
   * @constructor
   */
  voegExhibitorToe(exhibitor: Exhibitor): Observable<Exhibitor> {
    return this.http
      .post(`${this._appUrl}/AddExhibitor/`, this.exhibitorToDTO(exhibitor))
      .pipe(map(Exhibitor.fromJSON));
  }

  /**
   * Makes call to the backend and updates an existing exhibitor
   *
   * @param exhibitor
   */
  updateExhibitor(exhibitor: Exhibitor): Observable<Exhibitor> {
    return this.http
      .put(`${this._appUrl}/UpdateExhibitor/${exhibitor.id}`, this.exhibitorToDTO(exhibitor))
      .pipe(map(Exhibitor.fromJSON));
  }

  exhibitorToDTO(exhibitor) {
    let exh = {
      "id": exhibitor.id,
      "name": exhibitor.name,
      "x": exhibitor.x,
      "y": exhibitor.y,
      "ExhibitorNumber": exhibitor.exhibitorNumber,
      "categoryIds": exhibitor.categories.map(e => e.id)
    };
    return exh;
  }
}
