var dataTableProductObjData;

$Product = {
    bindProductEvents: function () {
        $("#btnExportCsvProduct").on("click", function () {
            var dataHref = $(this).attr('data-href');
            var searchText = $('.dataTables_filter input[type="search"]').val().trim();
            var locationHref = dataHref + '?searchText=' + searchText;
            window.location.href = locationHref;
        });
    },
    loadDataTables: function (dataTableId, iDisplayLength, sAjaxSourceUrl) {

        $.fn.dataTable.ext.errMode = () => alert('We are facing some problem while processing the current request. Please try again later.');

        dataTableProductObjData = $('#' + dataTableId).on('preXhr.dt', function (e, settings, data) {
            console.log('ajax start');
        }).on('xhr.dt', function (e, settings, json, xhr) {
            console.log('ajax end');
        }).DataTable({
            "bJQueryUI": true,
            "bAutoWidth": true,
            "sPaginationType": "full_numbers",
            "bPaginate": true,
            "iDisplayLength": iDisplayLength,
            "bSort": true,
            "bFilter": true,
            "bSortClasses": false,
            "lengthChange": false,
            "oLanguage": {
                "sLengthMenu": "Display _MENU_ records per page",
                "sZeroRecords": "Data not found.",
                "sInfo": "Page _START_ to _END_ (about _TOTAL_ results)",
                "sInfoEmpty": "Page 0 to 0 (about 0 results)",
                "sInfoFiltered": "",
                "sProcessing": "Loading"
            },
            "bProcessing": true,
            "bServerSide": true,
            "initComplete": function (settings, json) {
                $Product.setDataTableSearch(dataTableId);
                $Product.bindDataTablesSearchBoxEvent();
            },

            ajax: sAjaxSourceUrl,
            columns: [
                {
                    name: 'Id',
                    data: 'id',
                    title: "Id",
                    sortable: false,
                    searchable: false,
                    visible: false
                },
                {
                    name: 'Name',
                    data: 'name',
                    title: "Product Name",
                    sortable: true,
                    searchable: false
                },
                {
                    name: 'Id',
                    data: "id",
                    title: "Actions",
                    sortable: false,
                    searchable: false,
                    className: "w-20",
                    "mRender": function (data, type, row) {

                        return '<a href="#" data-client-id="' + row.id + '" data-name="' + row.name + '" title="View" class="btn btn-info btn-sm"><i class="fa fa-search"></i> View</a>'
                            + ' <a href="/Product/Edit/' + row.id + '\" data-href=\"/Product/Edit/' + row.id + '\" data-name="' + row.fullName + '" data-client-id="' + row.id + '" title="Edit" class="btn btn-info btn-sm"><i class="fa fa-edit"></i> Edit</a>'
                            + ' <a href="#"  data-name="' + row.name + '" data-client-id="' + row.id + '" title="Delete" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i> Delete</a>'

                    }
                }
            ]

        });

    },
    setDataTableSearch: function (dataTableId) {
        //App.LoaderShow();
        var filterInput = '#' + dataTableId + '_filter label input';
        $(filterInput).attr("placeholder", "Search by keyword");
        //App.LoaderHide();
    },
    bindDataTablesSearchBoxEvent: function () {

        var searchBoxId = '.dataTables_filter input[type="search"]';
        $(searchBoxId).unbind() // Unbind previous default bindings
            .bind("input", function (e) { // Bind our desired behavior
                
                // If the length is 3 or more characters, or the user pressed ENTER, search
                if (this.value.length >= 3 || e.keyCode == 13) {
                    // Call the API search function
                    dataTableProductObjData.search(this.value).draw();
                }
                // Ensure we clear the search if they backspace far enough
                if (this.value == "") {
                    dataTableProductObjData.search("").draw();
                }

                return;
            });

    },
    loadDNTCaptcha: function (contentId) {

        if ($('#' + contentId).length > 0) {

            $.ajax({
                url: "/Home/DNTCaptchaPartial",
                type: 'GET',
                dataType: "html",
                beforeSend: function () {
                    //App.LoaderShow();
                },
                success: function (result) {
                    //App.LoaderHide();
                    if (result != undefined || result != null) {
                        if ($('#' + contentId).length > 0) {
                            $('#' + contentId).html('');
                            $('#' + contentId).html(result);
                            $('#' + contentId).find('#dntCaptchaRefreshButton').attr('href', '');
                            $('#' + contentId).show();
                            $Product.bindDNTCaptchaEvent(contentId);
                        }

                    }
                },
                error: function (error) {
                    //App.LoaderHide();
                    console.log(error);
                }

            });

        }
    },
    loadDNTCaptchaJson: function (contentId) {

        if ($('#' + contentId).length > 0) {

            $.ajax({
                url: "/Home/DNTCaptchaData",
                type: 'GET',
                beforeSend: function () {
                    //App.LoaderShow();
                },
                success: function (response) {
                    //App.LoaderHide();
                    if (response != undefined || response != null) {
                        console.log(response);
                        const {
                            dntCaptchaImgUrl,
                            dntCaptchaId,
                            dntCaptchaTextValue,
                            dntCaptchaTokenValue,
                        } = response;
                        $("#dntCaptchaImg").attr("src", dntCaptchaImgUrl);
                        $("#DNTCaptchaText").attr("value", dntCaptchaTextValue);
                        $("#DNTCaptchaToken").attr("value", dntCaptchaTokenValue);
                        $("div.dntCaptcha").attr("id", dntCaptchaId);
                        $('#' + contentId).show();

                    }
                },
                error: function (error) {
                    //App.LoaderHide();
                    console.log(error);
                }

            });

        }
    },
    bindDNTCaptchaEvent: function (contentId) {

        $("#dntCaptchaRefreshButton").on("click", function () {

            var ajaxUrl = $(this).attr('data-ajax-url');
            $.ajax({
                url: ajaxUrl,
                type: 'GET',
                dataType: "html",
                beforeSend: function () {
                    //App.LoaderShow();
                },
                success: function (result) {
                    //App.LoaderHide();
                    if (result != undefined || result != null) {
                        if ($('#' + contentId).length > 0) {
                            $('#' + contentId).html('');
                            $('#' + contentId).html(result);

                            $('#' + contentId).find('#dntCaptchaRefreshButton').attr('href', '');

                            $('#' + contentId).show();

                            $Product.bindDNTCaptchaEvent(contentId);
                        }

                    }
                },
                error: function (error) {
                    //App.LoaderHide();
                    console.log(error);
                }

            });

            return false;
        });

    },
    refreshDNTCaptcha: function () {

        $("#DNTCaptchaInputText").val("");
        $Product.loadDNTCaptchaJson('dntCaptchaContent2');

    },
    loadCaptcha: function (contentId) {

        if ($('#' + contentId).length > 0) {

            $.ajax({
                url: "/Home/AppCaptchaData",
                type: 'GET',
                beforeSend: function () {
                    //App.LoaderShow();
                },
                success: function (response) {
                    //App.LoaderHide();
                    if (response != undefined || response != null) {
                        console.log(response);
                        const {
                            captchaId,
                            captchaCode,
                            captchaImage,
                            captchaToken,
                        } = response;
                        $("#CaptchaImage").attr("src", captchaImage);
                        $("#CaptchaId").attr("value", captchaId);
                        $("#CaptchaToken").attr("value", captchaToken);
                        $("div.Captcha").attr("id", captchaId);
                        $('#' + contentId).show();

                    }
                },
                error: function (error) {
                    //App.LoaderHide();
                    console.log(error);
                }

            });

        }
    },
    refreshCaptcha: function () {

        $("#CaptchaText").val("");
        $Product.loadCaptcha('CaptchaContent');

    }
}