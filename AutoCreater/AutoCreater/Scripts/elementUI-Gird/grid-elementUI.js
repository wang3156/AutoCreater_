

(function () {
    Vue.component('el-grid', {
        template: '<el-container class="el-grid" v-loading="loading">\
			         <el-main>\
                         <el-table ref="mainTable" v-bind:row-class-name="rowClassName"   v-bind:default-sort="defaultSort" v-bind:data="girdData" v-on:sort-change="onSortChange" stripe height="100%" style="width:100%;"   v-bind:cell-class-name="cellClassName">\
				             <slot>\
				             </slot>\
                         </el-table>\
			         </el-main>\
			         <el-footer class="page">\
				         <el-pagination v-bind:current-page="currentPage"\
                                        v-bind:page-sizes="computedPageSize"\
                                        v-bind:page-size="pageSize"\
                                        layout="total, sizes, prev, pager, next"\
                                        v-bind:total="totalRecords"\
                                        ref="gridPager">\
                        </el-pagination>\
			        </el-footer>\
		         </el-container>',
        props: {
            //默认排序
            defaultSort: {
                type: Object,
                required: true
            },
            //默认每页大小
            defaultPageSize: {
                type: Number,
                default: 20
            },
            pageSizes: {
                type: Array,
                default: function () {
                    return [10, 20, 30, 50, 100];
                }
            },
            //数据源URL
            url: {
                type: String,
                required: false
            },
            //过滤条件
            filter: {
                type: Object,
                default: function () {
                    return {};
                }
            },
            rowClassName: {
                type: Function,
                default: function () {
                    return ""
                }
            },
            cellClassName: {
                type: Function,
                default: function () {
                    return ""
                }
            }
        },
        data: function () {
            return {
                currentPage: 1,
                pageSize: this.defaultPageSize,
                totalRecords: 0,
                girdData: [],
                sort: JSON.parse(JSON.stringify(this.defaultSort)),
                loading: false
            };
        },
        computed: {
            computedPageSize: function () {
                //如果设置的条数不存在则加入到列表中
                if (this.pageSizes.indexOf(this.defaultPageSize) == -1) {
                    this.pageSizes = [];
                    for (var i = 1; i < 6; i++) {
                        this.pageSizes.push(this.defaultPageSize * i);
                    }                    
                }
                return this.pageSizes;
            }
        },
        methods: {
            //加载数据
            reload: function (pageInfo) {
                var actualPageInfo = this.buildPageInfoSmartly(pageInfo);
                this.currentPage = actualPageInfo.PageIndex;
                this.pageSize = actualPageInfo.PageSize;
                this.loadData(actualPageInfo);
            },
            buildPageInfoSmartly: function (pageInfo) {
                var retPageInfo = null;
                if (!pageInfo) {
                    retPageInfo = {
                        PageSize: this.pageSize,
                        PageIndex: 1,
                        Sort: this.sort
                    };
                }
                else {
                    if (pageInfo.PageIndex == null) { pageInfo.PageIndex = this.currentPage; };
                    if (pageInfo.PageSize == null) { pageInfo.PageSize = this.pageSize; };
                    if (pageInfo.Sort == null) {
                        pageInfo.Sort = this.sort;
                    };
                    retPageInfo = pageInfo;
                }
                return retPageInfo;
            },
            onSortChange: function (column) {
                this.sort.prop = column.prop;
                this.sort.order = column.order;
                this.reload({});
            },
            loadData: function (pageInfo) {
                var self = this;
                if (pageInfo.Sort.prop) {
                    pageInfo.SortExpression = pageInfo.Sort.prop + ' ' + (pageInfo.Sort.order == 'descending' ? 'desc' : 'asc');
                    delete pageInfo.Sort;
                }
                else {
                    pageInfo.SortExpression = this.defaultSort.prop ? (this.defaultSort.prop + ' ' + (this.defaultSort.order == 'descending' ? 'desc' : 'asc')) : '';
                }
                var data = { Filter: self.filter, PageInfo: pageInfo, _t: Math.random() };
                var done = function (result) {
                    self.totalRecords = result.TotalRowNum || 0;
                    self.girdData = result.Data || [];
                    self.$emit('load', result, self);
                };
                if (this.url) {
                    ajaxFun(this.url, data, done);
                   
                } else {
                    self.$emit('getdata',data,done);
                }
            },
            getSelection: function () {
                return this.$refs.mainTable.selection;
            },
            getColumns: function () {
                return this.$refs.mainTable.columns;
            }

        },
        mounted: function () {
            var self = this;
            var column = this.$refs.mainTable.columns.find(function (v) { return v.property == self.defaultSort.prop; });
            if (column && column.sortOrders.length == 3) {
                column.sortOrders = ['ascending', 'descending'];
            }
            this.$nextTick(function () {

                self.$refs.mainTable.$on('expand-change', function (row, expandedRows) {
                    self.$emit('expand-change', row, expandedRows);
                });
                self.$refs.mainTable.$on('row-click', function (row, event, column) {
                    self.$emit('row-click', row, column, event);
                });

                self.$refs.gridPager.$on('current-change', function (currentPage) {
                    self.currentPage = currentPage;
                    self.loadData({
                        PageIndex: self.currentPage,
                        PageSize: self.pageSize,
                        Sort: self.sort
                    });
                });

                self.$refs.gridPager.$on('size-change', function (pageSize) {
                    self.pageSize = pageSize;
                    self.currentPage = 1;
                    var pageInfo = {
                        PageIndex: self.currentPage,
                        PageSize: self.pageSize,
                        Sort: self.sort
                    };
                    self.loadData(pageInfo);
                });

                if (self.url) {
                    self.reload();
                }
            });
        }
    });

})(window, Vue, Element);