//add new Item Master to the db
$("#btnSave").click(function () {
    var Name = $("#name").val();
    var BatchNo = $("#BatchNo").val();
    var GenericDrugName = $("#GenericDrugName").val();
    var MfgCoName = $("#MfgCoName").val();
    var category = $("#category").val();
    var supplier = $("#supplier").val();
    var CurentStock = $("#CurentStock").val();
    var MfgDate = $("#MfgDate").val();
    var ICDCode = $("#ICDCode").val();
    var ReorderLevel = $("#ReorderLevel").val();
    var barcode = $("#barcode").val();
    var unitsPack = $("#unitsPack").val();
    var casePackRate = $("#casePackRate").val();
    var costPriceUnit = $("#costPriceUnit").val();
    var sellingPriceUnit = $("#sellingPriceUnit").val();
    var expiryStatus = $("#expiryStatus").val();
    var purchaseDate = $("#purchaseDate").val();

    var model = {
        ItemName: Name, BatchNo: BatchNo, GenericDrugName: GenericDrugName,
        MfgCoName: MfgCoName, Category: category, Supplier: supplier, CurrentStock: CurentStock,
        MfgDate: MfgDate, ICDTenCode: ICDCode, ReorderLevel: ReorderLevel, barCode: barcode,
        UnitsPack: unitsPack, CasePackRate: casePackRate, CostPriceUnit: costPriceUnit,
        SellingPriceUnit: sellingPriceUnit, ExpiryStatus: expiryStatus, PurchaseDate: purchaseDate
    }

    //Initialize a bool variable to false
    var boolVal = false;

    //run function to loop through all the values in the json
    //if any of the  values in the loop is missing then show assign boolVal to true
    $.each(model, function (key, value) {
        if (value == "") {
            boolVal = true;
            return boolVal;
        }
    });

    if (boolVal == true) {
        var result = $("#result");
        var string = '<div class="alert alert-warning">' +
            '<strong>Warning!</strong> One of your fields is missing a value.' +
            ' </div >';

        result.empty();
        result.append(string);
        //here the div containing the label successful is hidden after two seconds
        result.fadeTo(2000, 1000).slideUp(500, function () {
            result.slideUp(500);
        })

    }

    else {
        console.log(model);

        $.ajax({
            type: 'POST',
            url: '/ProcurementMaster/ItemMaster',
            data: model,
            success: function () {

                $("#formItemMaster")[0].reset();

                var result = $("#result");
                var string = '<div class="alert alert-success">' +
                    '<strong>Successful!</strong>Your Data was saved successfully.' +
                    ' </div >';

                result.empty();
                result.append(string);
                //here the div containing the label successful is hidden after two seconds
                result.fadeTo(2000, 1000).slideUp(500, function () {
                    result.slideUp(500);
                })

            },
            error: function () {
                var result = $("#result");
                var string = '<div class="alert alert-warning">' +
                    '<strong>Warning!</strong> An Error Occured While Saving Your Data.' +
                    ' </div >';

                result.empty();
                result.append(string);
                //here the div containing the label successful is hidden after two seconds
                result.fadeTo(2000, 1000).slideUp(500, function () {
                    result.slideUp(500);
                })

            }
        });
    }
})

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
                url: '/MinorMasterProcurement/AddNewGenericDrug',
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
                url: '/MinorMasterProcurement/AddNewCompany',
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
                url:'/MinorMasterProcurement/AddNewCategory',
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

//On key up populate the div for selecting DrugsList
$("#GenericDrugName").keyup(function () {

    var drugName = $("#GenericDrugName").val();
    var drugList = $("#GenericDrugNameList");

    if (drugName.length < 1) {
        drugList.fadeOut();
    }
    else {

        var nam = { name: drugName };

        $.ajax({
            type: 'POST',
            url: '/MinorMasterProcurement/SearchGenericDrug',
            data: nam,
            success: function (obj) {

                //create an array to store the values searched 
                var item = [];
                //loop through all the items and push then into the array variable

                $.each(obj, function (key, value) {
                    var string = '<p class="searchedItems" data-value="' + value.Name + '">' + value.Name + '</p>';
                    item.push(string);

                })

                // empty the div element
                $("#GenericDrugNameList").empty();

                var items = item.join("");
                drugList.append(items);
                drugList.fadeIn();

                $(".searchedItems").each(function () {
                    $(this).click(function () {
                        var val = $(this).data("value");
                        $("#GenericDrugName").val(val);
                        drugList.fadeOut();
                    })
                });


            },
            error: function () {
                console.log("Error Occured");
            }
        })

    }


})

//On key up to populate the div containing MgfList
$("#MfgCoName").keyup(function () {

    var ManfactureName = $("#MfgCoName").val();
    var ManufactureList = $("#MfgNameList");


    if (ManfactureName.length < 1) {

        ManufactureList.fadeOut();
    }
    else {

        var nam = { name: ManfactureName };

        $.ajax({
            type: 'POST',
            url: '/MinorMasterProcurement/SearchManufactureName',
            data: nam,
            success: function (obj) {

                //create an array to store the values searched 
                var item = [];
                //loop through all the items and push then into the array variable

                $.each(obj, function (key, value) {
                    var string = '<p class="searchedItems" data-value="' + value.Name + '">' + value.Name + '</p>';
                    item.push(string);

                })

                // empty the div element
                $("#MfgNameList").empty();

                var items = item.join("");
                ManufactureList.append(items);
                ManufactureList.fadeIn();

                $(".searchedItems").each(function () {
                    $(this).click(function () {
                        var val = $(this).data("value");
                        $("#MfgCoName").val(val);
                        ManufactureList.fadeOut();
                    })
                });


            },
            error: function () {
                console.log("Error Occured");
            }
        })

    }

})

//On key up to populate the div containing CategoryList
$("#category").keyup(function () {

    var categoryName = $("#category").val();
    var categoryList = $("#CategoryList");


    if (categoryName.length < 1) {

        categoryList.fadeOut();
    }
    else {

        var nam = { name: categoryName };

        $.ajax({
            type: 'POST',
            url: '/MinorMasterProcurement/SearchCategory',
            data: nam,
            success: function (obj) {

                //create an array to store the values searched 
                var item = [];
                //loop through all the items and push then into the array variable

                $.each(obj, function (key, value) {
                    var string = '<p class="searchedItems" data-value="' + value.CategoryName + '">' + value.CategoryName + '</p>';
                    item.push(string);

                })

                // empty the div element
                $("#CategoryList").empty();

                var items = item.join("");
                categoryList.append(items);
                categoryList.fadeIn();

                $(".searchedItems").each(function () {
                    $(this).click(function () {
                        var val = $(this).data("value");
                        $("#category").val(val);
                        categoryList.fadeOut();
                    })
                });


            },
            error: function () {
                console.log("Error Occured");
            }
        })

    }

})

//On key up to populate the div containing Supplier List
$("#supplier").keyup(function () {

    var supplierName = $("#supplier").val();
    var supplierList = $("#SupplierList");


    if (supplierName.length < 1) {

        supplierList.fadeOut();
    }
    else {

        var nam = { name: supplierName };

        $.ajax({
            type: 'POST',
            url: '/MinorMasterProcurement/SearchSupplier',
            data: nam,
            success: function (obj) {

                //create an array to store the values searched 
                var item = [];
                //loop through all the items and push then into the array variable

                $.each(obj, function (key, value) {
                    var string = '<p class="searchedItems" data-value="'+ value.Supplier_Name +'">' + value.Supplier_Name + '</p>';
                    item.push(string);

                })

                // empty the div element
                $("#SupplierList").empty();

                var items = item.join("");
                supplierList.append(items);
                supplierList.fadeIn();

                $(".searchedItems").each(function () {
                    $(this).click(function () {
                        var val = $(this).data("value");
                        $("#supplier").val(val);
                        supplierList.fadeOut();
                        supplierList.empty();
                    })
                });


            },
            error: function () {
                console.log("Error Occured");
            }
        })

    }

})

//On key up to populate the div containing drug list
$("#name").keyup(function () {

    var itemName = $("#name").val();
    var itemList = $("#ItemNameList");


    if (itemName.length < 1) {

        itemList.fadeOut();
    }
    else {

        var nam = { name: itemName };

        $.ajax({
            type: 'POST',
            url: '/MinorMasterProcurement/SearchItemList',
            data: nam,
            success: function (obj) {
                console.log(obj);
                //create an array to store the values searched 
                var item = [];
                //loop through all the items and push then into the array variable

                $.each(obj, function (key, value) {
                    var string = '<p class="searchedItems" data-value="' + value + '">' + value + '</p>';
                    item.push(string);

                })

                // empty the div element
                $("#ItemNameList").empty();

                var items = item.join("");
                itemList.append(items);
                itemList.fadeIn();

                $(".searchedItems").each(function () {
                    $(this).click(function () {
                        var val = $(this).data("value");
                        $("#name").val(val);
                        $("#ItemNameList").fadeOut();
                        $("#ItemNameList").empty();
                    })
                });


            },
            error: function (e, x, y) {
                console.log(e.responseText);
            }
        })

    }

})