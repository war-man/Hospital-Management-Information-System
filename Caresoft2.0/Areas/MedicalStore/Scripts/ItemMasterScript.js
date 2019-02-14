

//show Generic Modal and Add generic in db
$("#AddNewGenericDrug").click(function () {
    $("#GenericDrug").modal('show');

    $("#btnSaveGenericDrug").click(function () {

        //get the value of the Name
        var Name = $("#DrugName").val();


        //if value is empty throw error otherwise initialize ajax call after sav button has been clicked
        if (Name == "") {

            var result = $("#resultGeneric");
            var string = '<div class="alert alert-warning">' +
                '<strong>Warning!</strong> An Error Occured.Please Add Name.' +
                ' </div >';
            result.empty();
            result.append(string);
            //here the div containing the label successful is hidden after two seconds
            result.fadeTo(2000, 1000).slideUp(500, function () {
                result.slideUp(500);
                result.empty();
            })
        }
        else {
            var nam = { name: Name };
            $.ajax({
                type: 'POST',
                url: '/Procurement/MinorMasterProcurement/AddNewGenericDrug',
                dataType: 'json',
                data: nam,
                success: function () {

                    var result = $("#resultGeneric");
                    var string = '<div class="alert alert-success">' +
                        '<strong>Successful!</strong> Added Successfully.' +
                        ' </div >';
                    result.empty();
                    result.append(string);
                    result.fadeTo(2000, 1000).slideUp(500, function () {
                        result.slideUp(500);
                        result.empty();
                    })

                    $("#DrugName").val("");

                },
                error: function () {
                    var result = $("#resultGeneric");
                    var string = '<div class="alert alert-warning">' +
                        '<strong>Warning!</strong> An Error Occured While Adding.' +
                        ' </div >';
                    result.empty();
                    result.append(string);
                    result.fadeTo(2000, 1000).slideUp(500, function () {
                        result.slideUp(500);
                        result.empty();
                    })
                },

            })
        }

    })

});

//show Company Modal and save company name in Db
$("#AddNewMfgCompany").click(function () {
    $("#MfgCompanyModal").modal('show');

    $("#btnSaveNewCompany").click(function () {

        //get the value of the Name
        var Name = $("#CompanyName").val();


        //if value is empty throw error otherwise initialize ajax call after sav button has been clicked
        if (Name == "") {

            var result = $("#resultCompany");
            var string = '<div class="alert alert-warning">' +
                '<strong>Warning!</strong> An Error Occured.Please Add Name.' +
                ' </div >';
            result.empty();
            result.append(string);
            //here the div containing the label successful is hidden after two seconds
            result.fadeTo(2000, 1000).slideUp(500, function () {
                result.slideUp(500);
                result.empty();
            })
        }
        else {
            var nam = { name: Name };
            $.ajax({
                type: 'POST',
                url: '/Procurement/MinorMasterProcurement/AddNewCompany',
                dataType: 'json',
                data: nam,
                success: function () {

                    var result = $("#resultCompany");
                    var string = '<div class="alert alert-success">' +
                        '<strong>Successful!</strong> Added Successfully.' +
                        ' </div >';
                    result.empty();
                    result.append(string);
                    result.fadeTo(2000, 1000).slideUp(500, function () {
                        result.slideUp(500);
                        result.empty();
                    })

                    $("#CompanyName").val("");

                },
                error: function () {
                    var result = $("#resultCompany");
                    var string = '<div class="alert alert-warning">' +
                        '<strong>Warning!</strong> An Error Occured While Adding.' +
                        ' </div >';
                    result.empty();
                    result.append(string);
                    result.fadeTo(2000, 1000).slideUp(500, function () {
                        result.slideUp(500);
                        result.empty();
                    })
                },

            })
        }

    })

});
//show Category Modal and save company name in Db
$("#AddNewMfgCategory").click(function () {
    $("#CategoryModal").modal('show');

    $("#btnSaveCategory").click(function () {

        //get the value of the Name
        var Name = $("#CategoryName").val();


        //if value is empty throw error otherwise initialize ajax call after sav button has been clicked
        if (Name == "") {

            var result = $("#resultCategory");
            var string = '<div class="alert alert-warning">' +
                '<strong>Warning!</strong> An Error Occured.Please Add Name.' +
                ' </div >';
            result.empty();
            result.append(string);
            //here the div containing the label successful is hidden after two seconds
            result.fadeTo(2000, 1000).slideUp(500, function () {
                result.slideUp(500);
                result.empty();
            })
        }
        else {
            var nam = { name: Name };
            $.ajax({
                type: 'POST',
                url:'/Procurement/MinorMasterProcurement/AddNewCategory',
                dataType: 'json',
                data: nam,
                success: function () {

                    var result = $("#resultCategory");
                    var string = '<div class="alert alert-success">' +
                        '<strong>Successful!</strong> Added Successfully.' +
                        ' </div >';
                    result.empty();
                    result.append(string);
                    result.fadeTo(2000, 1000).slideUp(500, function () {
                        result.slideUp(500);
                        result.empty();
                    })

                    $("#CategoryName").val("");

                },
                error: function () {
                    var result = $("#resultCategory");
                    var string = '<div class="alert alert-warning">' +
                        '<strong>Warning!</strong> An Error Occured While Adding.' +
                        ' </div >';
                    result.empty();
                    result.append(string);
                    result.fadeTo(2000, 1000).slideUp(500, function () {
                        result.slideUp(500);
                        result.empty();
                    })
                },

            })
        }

    })

});


$("#name").autocomplete({
    source: function (request, response) {
        $.ajax({
            type: 'POST',
            url: "/Procurement/MinorMasterProcurement/SearchDrugList",
            dataType: "json",
            data: { name: $('#name').val() },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.Name, value: item.Name, selectedId: item.Id };
                }));
            }
        });
    },
    error: function (xmlhttprequest, textstatus, errorthrown) {
        showNotification("An Error Occured, try again later", "danger", true);
    },
    minLength: 2,
    select: function (event, ui) {
        $('#DrugId').val(ui.item.selectedId);
        SearchDrugById(ui.item.selectedId);
        console.log(ui.item.selectedId);
    }
});

$("#GenericDrugName").autocomplete({
    source: function (request, response) {
        $.ajax({
            type: 'POST',
            url: "/Procurement/MinorMasterProcurement/SearchGenericDrug",
            dataType: "json",
            data: { name: $('#GenericDrugName').val() },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.Name, value: item.Name, selectedId: item.Name };
                }));
            }
        });
    },
    error: function (xmlhttprequest, textstatus, errorthrown) {
        showNotification("An Error Occured, try again later", "danger", true);
    },
    minLength: 2,
    select: function (event, ui) {
        $("#GenericDrugName").val(ui.item.selectedId);
        console.log(ui.item.selectedId);
    }
});

$("#MfgCoName").autocomplete({
    source: function (request, response) {
        $.ajax({
            type: 'POST',
            url: "/Procurement/MinorMasterProcurement/SearchManufactureName",
            dataType: "json",
            data: { name: $('#MfgCoName').val() },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.Name, value: item.Name, selectedId: item.Id };
                }));
            }
        });
    },
    error: function (xmlhttprequest, textstatus, errorthrown) {
        showNotification("An Error Occured, try again later", "danger", true);
    },
    minLength: 2,
    select: function (event, ui) {
        $("#MfgCoName").val(ui.item.selectedId);
        console.log(ui.item.selectedId);
    }
});

$("#category").autocomplete({
    source: function (request, response) {
        $.ajax({
            type: 'POST',
            url: '/Procurement/MinorMasterProcurement/SearchCategory',
            dataType: "json",
            data: { name: $('#category').val() },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.CategoryName, value: item.CategoryName, selectedId: item.CategoryName };
                }));
            }
        });
    },
    error: function (xmlhttprequest, textstatus, errorthrown) {
        showNotification("An Error Occured, try again later", "danger", true);
    },
    minLength: 2,
    select: function (event, ui) {

        $("#category").val(ui.item.selectedId);
        console.log(ui.item.selectedId);
    }
});

$("#supplier").autocomplete({
    source: function (request, response) {
        $.ajax({
            type: 'POST',
            url: '/Procurement/MinorMasterProcurement/SearchSupplier',
            dataType: "json",
            data: { name: $('#supplier').val() },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.Supplier_Name, value: item.Supplier_Name, selectedId: item.Supplier_Name };
                }));
            }
        });
    },
    error: function (xmlhttprequest, textstatus, errorthrown) {
        showNotification("An Error Occured, try again later", "danger", true);
    },
    minLength: 2,
    select: function (event, ui) {

        $("#supplier").val(ui.item.selectedId);
        console.log(ui.item.selectedId);
    }
});

$("#supplier").autocomplete({
    source: function (request, response) {
        $.ajax({
            type: 'POST',
            url: "/Procurement/MinorMasterProcurement/SearchSupplier",
            dataType: "json",
            data: { name: $('#supplier').val() },
            success: function (data) {
                response($.map(data, function (item) {
                    return {
                        label: item.Supplier_Name, value: item.Supplier_Name,
                        selectedId: item.Supplier_Name
                    };
                }));
            }
        });
    },
    error: function (xmlhttprequest, textstatus, errorthrown) {
        showNotification("An Error Occured, try again later", "danger", true);
    },
    minLength: 2,
    select: function (event, ui) {
        $("#supplier").val(ui.item.selectedId);
        console.log(ui.item.selectedId);
    }
});

function SearchDrugById(Id) {

    var url = "/Procurement/MinorMasterProcurement/SearchDrugById?Id=" + Id;
    $('.loader').show();
    $.ajax({
        type: "GET",
        url: url,
        success: function (obj) {
            //console.log(obj);
            $("#GenericDrugName").val(obj.Name);
            $("#category").val(obj.CategoryName);
            $("#ReorderLevel").val(obj.ReorderLevel);
            $("#unitsPack").val(obj.UnitsPack);
            $('.loader').hide();

        }, error: function () {
            showNotification("AN Error Occured", "danger", true);
            $('.loader').hide();
        }
    });
}

//On key up populate the div for selecting DrugsList


//On key up to populate the div containing MgfList
//On key up to populate the div containing CategoryList


//On key up to populate the div containing Supplier List
//$("#supplier").keyup(function () {

//    var supplierName = $("#supplier").val();
//    var supplierList = $("#SupplierList");


//    if (supplierName.length < 1) {

//        supplierList.fadeOut();
//    }
//    else {

//        var nam = { name: supplierName };

//        $.ajax({
//            type: 'POST',
//            url: '/MinorMasterProcurement/SearchSupplier',
//            data: nam,
//            success: function (obj) {

//                //create an array to store the values searched 
//                var item = [];
//                //loop through all the items and push then into the array variable

//                $.each(obj, function (key, value) {
//                    var string = '<p class="searchedItems" data-value="'+ value.Supplier_Name +'">' + value.Supplier_Name + '</p>';
//                    item.push(string);

//                })

//                // empty the div element
//                $("#SupplierList").empty();

//                var items = item.join("");
//                supplierList.append(items);
//                supplierList.fadeIn();

//                $(".searchedItems").each(function () {
//                    $(this).click(function () {
//                        var val = $(this).data("value");
//                        $("#supplier").val(val);
//                        supplierList.fadeOut();
//                        supplierList.empty();
//                    })
//                });


//            },
//            error: function () {
//                console.log("Error Occured");
//            }
//        })

//    }

//})

//On key up to populate the div containing drug list
//$("#name").keyup(function () {

//    var itemName = $("#name").val();
//    var itemList = $("#ItemNameList");


//    if (itemName.length < 1) {

//        itemList.fadeOut();
//    }
//    else {

//        var nam = { name: itemName };

//        $.ajax({
//            type: 'POST',
//            url: '/MinorMasterProcurement/SearchItemList',
//            data: nam,
//            success: function (obj) {
//                console.log(obj);
//                //create an array to store the values searched 
//                var item = [];
//                //loop through all the items and push then into the array variable

//                $.each(obj, function (key, value) {
//                    var string = '<p class="searchedItems" data-value="' + value + '">' + value + '</p>';
//                    item.push(string);

//                })

//                // empty the div element
//                $("#ItemNameList").empty();

//                var items = item.join("");
//                itemList.append(items);
//                itemList.fadeIn();

//                $(".searchedItems").each(function () {
//                    $(this).click(function () {
//                        var val = $(this).data("value");
//                        $("#name").val(val);
//                        $("#ItemNameList").fadeOut();
//                        $("#ItemNameList").empty();
//                    })
//                });


//            },
//            error: function (e, x, y) {
//                console.log(e.responseText);
//            }
//        })

//    }

//})