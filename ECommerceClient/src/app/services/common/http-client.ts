import {Inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HttpClientService {
  constructor(private httpClient: HttpClient, @Inject("baseUrl") private baseUrl: string) {}

  get<T>(){
    let url: string = ""; 
  }

  post(){
  }

  put(){

  }

  delete(){

  }

}
