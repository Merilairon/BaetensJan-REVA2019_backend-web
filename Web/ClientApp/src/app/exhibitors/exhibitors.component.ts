import {Component, OnInit, TemplateRef} from '@angular/core';
import {BsModalRef, BsModalService, TypeaheadMatch} from "ngx-bootstrap";
import {Router} from "@angular/router";
import {ExhibitorsDataService} from "./exhibitors-data.service";
import {ExhibitorShareService} from "../exhibitor/exhibitor-share.service";
import {Exhibitor} from "../models/exhibitor.model";

@Component({
  selector: 'app-exhibitors',
  templateUrl: './exhibitors.component.html',
  styleUrls: ['./exhibitors.component.css']
})
export class ExhibitorsComponent implements OnInit {
  /**
   * @ignore
   */
  private _exhibitors: Exhibitor[];
  /**
   * @ignore
   */
  private _contentArray: Exhibitor[];
  /**
   * @ignore
   */
  clickedItem: Exhibitor;
  /**
   * @ignore
   */
  refModal: BsModalRef;

  /**
   * Constructor
   *
   * @param router: Router
   * @param modalService: BsModalService
   * @param _exhibitorsDataService: ExhibitorsDataService
   * @param _exhibitorShareService: ExhibitorsShareService
   */
  ngOnInit() {
    this._exhibitorsDataService.exhibitors.subscribe(exhibitors => {
      this._exhibitors = exhibitors.sort((a, b) => a.name > b.name ? 1 : -1);
      this._contentArray = this._exhibitors;
    });
  }

  constructor(private router: Router,
              private modalService: BsModalService,
              private _exhibitorsDataService: ExhibitorsDataService,
              private _exhibitorShareService: ExhibitorShareService
  ) {
  }

  /**
   * On enter of click on typeAHeadMatch, the chosen exhibitor will be displayed.
   * @param event
   */
  onSelect(event: TypeaheadMatch): void {
    let exhibitor = event.item;
    this._contentArray = [];
    this._contentArray.push(exhibitor);
  }

  /**
   * No result matching the searchterm in the filter.
   * @param event
   */
  typeaheadNoResults(event: boolean): void {
    this._contentArray = this._exhibitors;
  }
  /**
   * Returns the filtered exhibitors
   *
   */
  get exhibitors() {
    return this._contentArray;
  }

  /**
   * Toggle voor modal te tonen.
   *
   */
  hideModal() {
    this.refModal.hide();
  }

  /**
   * Click event om exhibitor toe te voegen. Geeft een lege exhibitor door aan share service om zo de data te kunnen verkrijgen in het exhibitor component.
   *
   */
  onToevoegenExhibitor() {
    this._exhibitorShareService.exhibitor = null;
    this.router.navigate((["/exposant"]))
  }

  /**
   * Click event om exhibitor aan te passen. Geeft de exhibitor door aan share service om zo de data te kunnen verkrijgen in het exhibitor component.
   *
   */
  onAanpassenExhibitor(row: Exhibitor) {
    this._exhibitorShareService.exhibitor = row;
    this._exhibitorShareService.aanpassen = true;
    this.router.navigate(["/exposant"]);
  }

  /**
   * Bevestiging van het verwijderen van een exhibitor. Dit wordt gebruikt bij het modal om te bevestigen.
   *
   */
  verwijderenBevestigd() {
    this.refModal.hide();
    this._exhibitorsDataService.verwijderExhibitor(this.clickedItem).subscribe(exhibitor => {
      let index = this._exhibitors.indexOf(this._exhibitors.find((c) => c.id === exhibitor.id));
      this._exhibitors.splice(index, 1);
      index = this._contentArray.indexOf(this._contentArray.find((c) => c.id === exhibitor.id));
      this._contentArray.splice(index, 1);
    });
    window.location.reload();
  }

  openModal(template: TemplateRef<any>, exhibitor: Exhibitor) {
    this.clickedItem = exhibitor;
    this.refModal = this.modalService.show(template, {class: 'modal-sm'});
  }
}