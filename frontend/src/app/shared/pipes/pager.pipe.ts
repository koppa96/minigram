import { Pipe, PipeTransform } from '@angular/core'
import { PagedList, PagerModel } from '../models/pager.model'
import { defaultPageSize } from '../constants'

@Pipe({
  pure: true,
  name: 'pager'
})
export class PagerPipe implements PipeTransform {
  transform(value: PagedList<any>): PagerModel {
    return (value
      ? {
        pageSize: value.pageSize,
        pageIndex: value.pageIndex,
        totalCount: value.totalCount,
        totalPages: Math.ceil(value.totalCount / value.pageSize)
      }
      : {
        pageSize: defaultPageSize,
        pageIndex: 0,
        totalPages: 0,
        totalCount: 0
      }
    )
  }
}
