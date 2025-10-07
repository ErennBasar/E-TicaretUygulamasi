import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { List_Product } from '../../../../contracts/list_product';
import { ProductService } from '../../../../services/common/models/product';
import { NgxSpinner, NgxSpinnerService } from 'ngx-spinner';
import { Base, SpinnerType } from '../../../../base/base';
import { AlertifyService, MessageType, Position } from '../../../../services/admin/alertify';
import { DeleteDirective } from '../../../../directives/admin/delete';

declare var $ : any;

@Component({
  selector: 'app-list',
  imports: [
    MatTableModule,
    MatPaginatorModule,
    DeleteDirective
  ],
  templateUrl: './list.html',
  styleUrl: './list.scss'
})
export class List extends Base implements OnInit {
  constructor(private productService: ProductService, spinner: NgxSpinnerService, private alertifyService: AlertifyService) {
    super(spinner);
  }

  displayedColumns: string[] = ['name', 'stock', 'price', 'createdDate', 'updatedDate','edit','delete'];
  dataSource: MatTableDataSource<List_Product> = null;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  // Verileri listelerken tüm verileri çekip listelemek yerine istenen sayıda veriyi listeme işlemi yaptım
 
  async getProducts(){
    this.showSpinner(SpinnerType.BALL_SPIN_CLOCKWİSE_FADE_ROTATING);
    const allProducts: { totalCount: number; products: List_Product[]} = await this.productService.read( this.paginator ? this.paginator.pageIndex : 0, this.paginator ? this.paginator.pageSize : 5,
      () => this.hideSpinner(SpinnerType.BALL_SPIN_CLOCKWİSE_FADE_ROTATING),
      errorMessage =>
        this.alertifyService.message(errorMessage, {
          dismissOthers: true,
          messageType: MessageType.ERROR,
          position: Position.BOTTOM_LEFT,
        })
    );
    this.dataSource = new MatTableDataSource<List_Product>(allProducts.products);
    this.paginator.length = allProducts.totalCount;
    //this.dataSource.paginator = this.paginator;
  }

  // delete(id, event){
  //   alert(id)
  //   const img: HTMLImageElement = event.srcElement;
  //   $(img.parentElement.parentElement).fadeOut(2000);
  // }

  async pageChanged(){
    await this.getProducts();
  }

  async ngOnInit() {
   await this.getProducts();
  }

}
