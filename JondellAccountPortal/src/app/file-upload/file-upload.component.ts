import { Component, OnInit } from '@angular/core';
import { AppService } from 'src/app/app.service';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit {


  afuConfig = {
    multiple: false,
    formatsAllowed: ".csv,.xlsx",
    maxSize: "1",
    uploadAPI:  {
      url: this.appService.baseUrl +  "/monthlyBalance/upload",
      headers: {
     "Authorization" : `Bearer ${localStorage.getItem("jwt")}`
      }
    },
};

  constructor(private appService: AppService) { }

  ngOnInit() {
  }



}
