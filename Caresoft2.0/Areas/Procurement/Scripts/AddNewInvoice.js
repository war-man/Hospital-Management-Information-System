//variables for new drug
var name = null;
var icdten = null;
var genericDrugName = null;
var category = null;
var unitpacks = null;
var reorderLevel = null;

//Modal for adding a new Drug
$("#AddNewDrug").click(function () {
        $("#NewDrugModal").modal('show');
    })

$("#btnSaveNewDrug").click(function () {
        GetVariableValues();
    var bool = false;

            var model = {
                Name: name, IcdTenCode: icdten, GenericDrugNameId: genericDrugName,
                Category: category, UnitsPack: unitpacks, ReorderLevel: reorderLevel
            }

            //console.log(model);

            $.each(model, function (key, value) {

                if (value == "")
                {
                    bool = true;
                }
            })

    if (bool == false)
    {
        $.ajax({
            type: 'POST',
            url: '/ProcurementPurchase/AddNewDrug',
            data: model,
            success: function () {
                ClearAddNewDrugTextBoxes();
                ShowSuccessfulResult();
            },
            error: function () {

                ShowErrorResult();
            }
        })
    }
    else
    {

        ErrorPleaseFillAllTextBoxes();
    }

  });

function GetVariableValues() {
        name = $("#drugName").val();
    icdten = $("#icdtencode").val();
            genericDrugName = $("#genericDrugName").data('value');
            category = $("#CategoryName").val();
            unitpacks = $("#unitpacks").val();
            reorderLevel = $("#reorderLevel").val();
        }

function ClearAddNewDrugTextBoxes() {
        $("#drugName").val("");
    $("#icdtencode").val("");
            $("#genericDrugName").val("")
            $("#CategoryName").val("");
            $("#unitpacks").val("");
            $("#reorderLevel").val("");
        }

function ShowSuccessfulResult() {
            var result = $("#resultAddNewDrug");
            var string = '<div class="alert alert-success">' +
                '<strong>Successful!</strong> Added Successfully.' +
                ' </div >';
            result.empty();
            result.append(string);
            result.fadeTo(2000, 1000).slideUp(500, function () {
        result.slideUp(500);
    result.empty();
            })

        }

function ErrorPleaseFillAllTextBoxes() {
            var result = $("#resultAddNewDrug");
            var string = '<div class="alert alert-warning">' +
                '<strong>Warning!</strong> Some Fields are missing.' +
                ' </div >';
            result.empty();
            result.append(string);
            result.fadeTo(2000, 1000).slideUp(500, function () {
        result.slideUp(500);
    result.empty();
            })

        }
function ShowErrorResult() {
            var result = $("#resultAddNewDrug");
            var string = '<div class="alert alert-warning">' +
                '<strong>Warning!</strong> An Error Occured While Adding.' +
                ' </div >';
            result.empty();
            result.append(string);
            result.fadeTo(2000, 1000).slideUp(500, function () {
        result.slideUp(500);
    result.empty();
            })
        }

$("#ItemName").autocomplete({
    source: function (request, response) {
        $.ajax({
            type: 'GET',
            url: '/ProcurementPurchase/SearchItemName',
            data: { name: $("#ItemName").val() },
            success: function (data) {

                console.log(data);
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
        $("#ItemName").val($(this).data('itemaname'));
        $("#UnitsPerCase").val($(this).data('unitspack'));
        $("#NoOfPacks").val(1);
        $("#Per").val(1);
        $("#FreeUnits").val(0);
        $("#vat_cst_percentage").val(0);

        //calculate number of Units
        var noOfPacks = $("#NoOfPacks").val();
        var unitsPerCase = $("#UnitsPerCase").val();
        var units = noOfPacks * unitsPerCase;
        $("#Units").val(units);
    }
});

$("#SupplierName").autocomplete({
    source: function (request, response) {
        $.ajax({
            type: 'POST',
            url: '/ProcurementPurchase/SearchSupplierName',
            data: { name: $("#SupplierName").val() },
            success: function (data) {

                console.log(data);
                response($.map(data, function (item) {
                    return { label: item.Supplier_Name, value: item.Supplier_Name, selectedId: item.Id };
                }));
            }
        });
    },
    error: function (xmlhttprequest, textstatus, errorthrown) {
        showNotification("An Error Occured, try again later", "danger", true);
    },
    minLength: 2,
    select: function (event, ui) {
       
    }
});


//On key up populate the div for selecting Supplier List
//$("#SupplierName").keyup(function () {

//            var SupplierName = $("#SupplierName").val();
//            var supplierList = $("#SupplierList");

//            if (SupplierName.length < 1) {
//        supplierList.fadeOut();
//    }
//            else {

//                var nam = {name: SupplierName };

//                $.ajax({
//                    type: 'POST',
//                    url: '/ProcurementPurchase/SearchSupplierName',
//                    data: nam,
//                    success: function (obj) {

//                        //create an array to store the values searched
//                        var item = [];
//                        //loop through all the items and push then into the array variable

//                        $.each(obj, function (key, value) {
//                            var string = '<p class="searchedItems" data-value="' + value.Supplier_Name + '">' + value.Supplier_Name + '</p>';
//                            item.push(string);

//                        })

//                        // empty the div element
//                        $("#SupplierList").empty();

//                        var items = item.join("");
//                        supplierList.append(items);
//                        supplierList.fadeIn();

//                        $(".searchedItems").each(function () {
//        $(this).click(function () {
//            var val = $(this).data("value");
//            $("#SupplierName").val(val);
//            supplierList.fadeOut();
//        })
//    });
//                    },
//                    error: function () {
//        console.log("Error Occured");
//    }
//                })

//            }


//        })

//On key up populate the div for selecting New drug for the invoice
//$("#ItemName").keyup(function () {

//            var itemName = $("#ItemName").val();
//            var itemList = $("#ItemList");

//            if (itemName.length < 1)
//            {
//               itemList.fadeOut();
//            }
//            else {

//                var nam = {name: itemName };

//                $.ajax({
//                    type: 'POST',
//                    url: '/ProcurementPurchase/SearchItemName',
//                    data: nam,
//                    success: function (obj) {

//                        //create an array to store the values searched
//                        var item = [];
//                        //loop through all the items and push then into the array variable

//                         //console.log(obj);
//                         $.each(obj, function (key, value) {
                             
//                            var string = '<p class="searchedItems" ';
//                             var moredata = 'data-unitspack="' + value.UnitsPack + '" ';
//                             var otherData = 'data-itemaname="' + value.Name + '" > ' + value.Name + '</p > ';

//                            var itm = string + moredata + otherData;
//                            item.push(itm);

//                        })

//                        // empty the div element
//                        $("#ItemList").empty();

//                        var items = item.join("");
//                        itemList.append(items);
//                        itemList.fadeIn();

//                        $(".searchedItems").each(function () {
//                            $(this).click(function () {
//                                $("#ItemName").val($(this).data('itemaname'));
//                                $("#UnitsPerCase").val($(this).data('unitspack'));
//                                $("#NoOfPacks").val(1);
//                                $("#Per").val(1);
//                                $("#FreeUnits").val(0);
//                                $("#vat_cst_percentage").val(0);

//                                //calculate number of Units
//                                var noOfPacks = $("#NoOfPacks").val();
//                                var unitsPerCase = $("#UnitsPerCase").val();
//                                var units = noOfPacks * unitsPerCase;
//                                $("#Units").val(units);

//                                itemList.fadeOut();
//                            })
//                        });
//                     },
//                    error: function () {
//                            console.log("Error Occured");
//                        }
//                 })

//                }


//})


//calculate vat
//var vatpercentage = $("#vat_cst_percentage").val();
//var Amount = $("#Amount").val();
//var vatAmount = (vatpercentage / 100) * Amount;
//$("#vatAmount").val(vatAmount);


//On key up populate the div for selecting Item List
$("#genericDrugName").keyup(function () {

    var genericDrugName = $("#genericDrugName").val();
    var genericDrugList = $("#genericDrugList");

    if (genericDrugName.length < 1) {
        genericDrugList.fadeOut();
    }
    else {

        var nam = { name: genericDrugName };

        $.ajax({
            type: 'POST',
            url: '/MinorMasterProcurement/SearchGenericDrug',
            data: nam,
            success: function (obj) {

                //create an array to store the values searched
                var item = [];
                //loop through all the items and push then into the array variable

                $.each(obj, function (key, value) {
                    var string = '<p class="searchedItems" data-title="' + value.Name + '" data-value="' + value.Id + '">' + value.Name + '</p>';
                    item.push(string);

                    console.log(obj);

                })

                // empty the div element
                $("#genericDrugList").empty();

                var items = item.join("");
                genericDrugList.append(items);
                genericDrugList.fadeIn();

                $(".searchedItems").each(function () {
                    $(this).click(function () {
                        var val = $(this).data('value');
                        var val2 = $(this).data('title');

                        $("#genericDrugName").val(val2);
                        $("#genericDrugName").data('value', val);
                        genericDrugList.fadeOut();
                    })
                });
            },
            error: function () {
                console.log("Error Occured");
            }
        })

    }


})

var Invoice = null;
var InvoiceDetails = null;
var checkIfModelIsValid = true;

$("#BtnSave").click(function () {
    var bool = false;
    InitializeInvoiceVariables();
    InitializeInvoiceDetailsVariables()
    
    var model = { invoice: Invoice, invoiceDetails: InvoiceDetails };

    //console.log(model);


    if (checkIfModelIsValid == true) {

        $.ajax({
            type: 'POST',
            url: '/ProcurementPurchase/AddNewInvoice',
            data: model,
            beforeSend: function () {
                $(".loader").show();
            },
            success: function (obj) {
                //console.log(obj);
                var invoiceDetailsList = $("#invoiceDetailsList");
                invoiceDetailsList.empty();
                invoiceDetailsList.append(obj);
                faclick();

                //function that populates summary below the table 
                populateSummary();

                //reset form
                $("#invoicedetails")[0].reset();
                $(".loader").hide();
                $("#showNotification").notify("Data Saved Successfully!", {
                    arrowShow: false,
                    showAnimation: 'fadeIn',
                    gap: 0,
                    className: 'success',
                });
                DisableInvoiceDetails();
            },
            error: function (e, x, f) {
                console.log(e.responseText);
                $("#showNotification").notify("Entry not Saved, Error Occured Saving entires", {
                    arrowShow: false,
                    showAnimation: 'fadeIn',
                    gap: 0
                });
                $(".loader").hide();
            }
        })
    }
    else {
        $("#showNotification").notify("Data not Saved, please check Your entires", {
            arrowShow: false,
            showAnimation: 'fadeIn',
            gap:0
        });
    }
   
})

function DisableInvoiceDetails() {
    $("#InvoiceNumber").prop("disabled", "disabled");
    $("#FinancialYear").prop("disabled", "disabled");
    $("#PoNumber").prop("disabled", "disabled");
    $("#PoDate").prop("disabled", "disabled");
    $("#InvoiceDate").prop("disabled", "disabled");
    $("#SupplierName").prop("disabled", "disabled");
    $("#TempInvoiceNumber").prop("disabled", "disabled");
    
}

function EnableInvoiceDetails() {
    $("#InvoiceNumber").prop("disabled", "enabled");
    $("#FinancialYear").prop("disabled", "enabled");
    $("#PoNumber").prop("disabled", "enabled");
    $("#PoDate").prop("disabled", "enabled");
    $("#InvoiceDate").prop("disabled", "enabled");
    $("#SupplierName").prop("disabled", "enabled");
    $("#TempInvoiceNumber").prop("disabled", "enabled");
}

function InitializeInvoiceVariables() {

    //variables for the Invoice Module
    var supplier_type = $("input:radio[name=optradio]:checked").val()
    var financial_year = $("#FinancialYear").val();
    var po_number = $("#PoNumber").val();
    var po_date = $("#PoDate").val();
    var invoice_date = $("#InvoiceDate").val();
    var invoice_number = $("#InvoiceNumber").val();
    var supplier_name = $("#SupplierName").val();
    var tempinvoice_number = $("#TempInvoiceNumber").val();

    Invoice = {
        SupplierType: supplier_type,
        FinancialYear: financial_year,
        PONumber: po_number,
        PODate: po_date,
        InvoiceDate: invoice_date,
        InvoiceNo: invoice_number,
        SupplierName: supplier_name,
        TempInvoiceNumber: tempinvoice_number
    };
    checkIfModelIsValid = true;
    $.each(Invoice, function (key, value) {
        if (key == "InvoiceNo")
        {
            if (value) {
                
            }
            else
            {
                $("#InvoiceNumber").notify("Required", { position: 'right' });
                checkIfModelIsValid = false;
            }
        }
        else if (key == "SupplierName") {
            if (value) {

            }
            else {
                $("#SupplierName").notify("Required", { position: 'right' });
                checkIfModelIsValid = false;
            }
        }
        else if (key == "InvoiceDate") {
            if (value) {

            }
            else {
                $("#InvoiceDate").notify("Required", { position: 'right' });
                checkIfModelIsValid = false;
            }
        }
    })
}

function InitializeInvoiceDetailsVariables() {

    //variables for the Invoice Details Section
    var _item = $("#ItemName").val();
    var batch_no = $("#BatchNo").val();
    var units_per_case = $("#UnitsPerCase").val();
    var no_of_pack = $("#NoOfPacks").val();
    var _units = $("#Units").val();
    var _per = $("#Per").val();
    var cost_price = $("#CostPrice").val();
    var _amount = $("#Amount").val();
    var llll = $("input:radio[name=discount]:checked").val()
    var free_units = $("#FreeUnits").val();
    var cost_per_case_pack = $("#CostPerCasePack").val();
    var mfg_date = $("#MfgDate").val();
    var expiry_status = $("#ExpiryStatus").val();
    var mgf_company = $("#MfgCompany").val();
    var mrp_pack = $("#MrpPack").val();
    var _discount = $("#actualDiscount").val();
    var vat_cst = $("input:radio[name=vat_cst]:checked").val()
    var vat_cst_on = $("#Vat_Cst").val();
    var vatAmount = $("#vatAmount").val();
    var freight_charges = $("#freightCharges").val();
    var packing_charges = $("#packingCharges").val();
    var StoreFlag = $("#StoreName").val();
    
    checkIfModelIsValid = true;

    InvoiceDetails = {
        Item: _item,
        BatchNo: batch_no,
        UnitsPerCase: units_per_case,
        NoOfPack: no_of_pack,
        Units: _units,
        per: _per,
        CostPrice: cost_price,
        Amount: _amount,
        FreeUnits: free_units,
        CostPerCasePack: cost_per_case_pack,
        MfgDate: mfg_date,
        ExpiryStatus: expiry_status,
        MfgCoNm: mgf_company,
        MRP: mrp_pack,
        Discount: _discount,
        VatAmt: vatAmount,
        Vat_Cst: vat_cst,
        VatCstOn: vat_cst_on,
        //VAT_CstPercentage: vat_cst_percentage,
        FreightCharges: freight_charges,
        PackingCharges: packing_charges,
        StoreFlag: StoreFlag
    };


    $.each(InvoiceDetails, function (key, value) {
        if (key == "Item") {
            if (value) {

            }
            else {
                $("#ItemName").notify("Required", { position: 'right' });
                checkIfModelIsValid = false;
            }
        }
        else if (key == "BatchNo") {
            if (value) {

            }
            else {
                $("#BatchNo").notify("Required", { position: 'right' });
                checkIfModelIsValid = false;
            }
        }
        else if (key == "MfgDate") {
            if (value) {

            }
            else {
                $("#MfgDate").notify("Required", { position: 'right' });
                checkIfModelIsValid = false;
            }
        }
        else if (key == "ExpiryStatus") {
            if (value) {

            }
            else {
                $("#ExpiryStatus").notify("Required", { position: 'right' });
                checkIfModelIsValid = false;
            }
        }
        else if (key == "MRP") {
            if (value) {

            }
            else {
                $("#MrpPack").notify("Required", { position: 'right' });
                checkIfModelIsValid = false;
            }
        }
        else if (key == "StoreFlag") {
            if (value) {

            }
            else {
                $("#StoreName").notify("Required", { position: 'right' });
                checkIfModelIsValid = false;
            }
        }
        
    })

}

function populateSummary() {
    $("#TAmount").val($("#totalsValues").data('linetotals'));
    $("#Tvat").val($("#totalsValues").data('vattotals'));
    $("#Tdiscount").val($("#totalsValues").data('discounttotals'));
    $("#AmountT").val($("#totalsValues").data('linetotals'));
    $("#grandtotal").val($("#totalsValues").data('grandtotal'));
}

$("#AddNewInvoice").click(function () {

    $.ajax({
        type: 'GET',
        url: '/ProcurementPurchase/AddNew',
        success: function (obj) {
            //resets the forms
            $("#InvoiceMain")[0].reset();
            $("#invoicedetails")[0].reset();
            $("#form0")[0].reset();

            //reset the list of invoice items
            var invoiceDetailsList = $("#invoiceDetailsList");
            invoiceDetailsList.empty();
            invoiceDetailsList.append(obj);

        },
        error: function (x, y, z) {

            console.log(x.responseText);
        }
    })


})

$("#DiscountGiven").keyup(function () {

    var discount = $("input:radio[name=discount]:checked").val()
    var amount = $("#Amount").val()
    var DiscountGiven = $("#DiscountGiven").val();

    if (DiscountGiven != "") {
        if (discount == "DiscountAmount") {
            $("#actualDiscount").val(DiscountGiven);

        }
        else if (discount = "DiscountPercentage") {
            var disamount = (DiscountGiven / 100) * amount;
            $("#actualDiscount").val(disamount);

        }
    }

});

$("#NoOfPacks").keyup(function () {

    //calculate number of units
    var noOfPacks = $("#NoOfPacks").val();
    var unitsPerCase = $("#UnitsPerCase").val();
    var units = noOfPacks * unitsPerCase;
    $("#Units").val(units);

    //calculate amount
    var costPrice = $("#CostPrice").val();
    var units = $("#Units").val();
    var amount = costPrice * units;
    $("#Amount").val(amount);

    var CostPerCasePack = (units / $("#NoOfPacks").val()) * costPrice;
    $("#CostPerCasePack").val(CostPerCasePack);

});

$("#UnitsPerCase").keyup(function () {
    //calculate number of units
    var noOfPacks = $("#NoOfPacks").val();
    var unitsPerCase = $("#UnitsPerCase").val();
    var units = noOfPacks * unitsPerCase;
    $("#Units").val(units);

    //calculate amount
    var costPrice = $("#CostPrice").val();
    var units = $("#Units").val();
    var amount = costPrice * units;
    $("#Amount").val(amount);

    var CostPerCasePack = (units / $("#NoOfPacks").val()) * costPrice;
    $("#CostPerCasePack").val(CostPerCasePack);
})

$("#CostPrice").keyup(function () {
    //calculate amount
    var costPrice = $(this).val();
    var units = $("#Units").val();
    var amount = costPrice * units;
    $("#Amount").val(amount);

    // case pack rate
    var CostPerCasePack = (units / $("#NoOfPacks").val()) * costPrice;
    $("#CostPerCasePack").val(CostPerCasePack);
});

$("#CostPerCasePack").keyup(function () {
    var costPerCasePack = $("#CostPerCasePack").val();

    var costprice = (costPerCasePack / $("#UnitsPerCase").val());

    $("#CostPrice").val(costprice);
    var units = $("#Units").val();
    var amount = costprice * units;
    $("#Amount").val(amount);

})

$("#vat_cst_percentage").keyup(function () {

    var vatpercentage = $("#vat_cst_percentage").val();
    var Amount = $("#Amount").val();
    var vatAmount = (vatpercentage / 100) * Amount;
    $("#vatAmount").val(vatAmount);

});

var SaveInvoiceMain = null;

function SummaryInvoiceVariables() {
    var _Amount = $("#TAmount").val();
    var _vatAmount = $("#Tvat").val();
    var _discount = $("#Tdiscount").val();
    var _other = $("#other").val();
    var _totalAmount = $("#AmountT").val();
    var _amountPage = $("#AmountPage").val();
    var _GrandTotal = $("#grandtotal").val();

    SaveInvoiceMain = {
        Amount: _Amount,
        vatAmount: _vatAmount,
        discount: _discount,
        other: _other,
        totalAmount: _totalAmount,
        amountPage: _amountPage,
        GrandTotal: _GrandTotal
    };

}

$("#TSave").click(function () {
    SummaryInvoiceVariables();

    $.ajax({
        type: 'POST',
        url: '/ProcurementPurchase/SaveButtonInvoiceMain',
        data: SaveInvoiceMain,
        beforeSend: function () {
            $(".loader").show();
        },
        success: function (obj) {
            //console.log(obj);
            $("#InvoiceMain")[0].reset();
            $("#invoicedetails")[0].reset();
            $("#form0")[0].reset();

            EnableInvoiceDetails();
            if (obj == "Invoice is null")
            {
                $("#ShowInvoiceNotification").notify("There is no invoice Data to save", "error");
            }
            else
            {
                var invoiceDetailsList = $("#invoiceDetailsList");
                invoiceDetailsList.empty();
                invoiceDetailsList.append(obj);
                $("#ShowInvoiceNotification").notify("Data Saved Successfully", "success");
            }
           
            $(".loader").hide();
        },
        error: function (x, y, z) {
            console.log(x.responseText);
            $(".loader").hide();
        }
    })
})

$("#TReport").click(function () {

    $.ajax({
        type: 'GET',
        url: '/ProcurementPurchase/GenerateReport',
        success: function () {
            console.log("Generating Report")
        },
        error: function (x, y, z) {

            console.log(x.responseText);
        }
    })
})

$("#ExpiryStatus").on('change', function () {

    var expiryStatus = $("#ExpiryStatus").val();
    if (expiryStatus == "ExpiryItem") {

        $("#expiryDate").removeClass("collapse");

    }
    else if (expiryStatus == "NonExpiryItem") {
        $("#expiryDate").addClass("collapse");
    }
    else {
        $("#expiryDate").addClass("collapse");
    }

})

