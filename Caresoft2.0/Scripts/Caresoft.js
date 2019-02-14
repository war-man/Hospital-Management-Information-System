var host = window.location.origin;


function preventModalBackdrop(modalId) {
    $('#' + modalId).modal({
        backdrop: 'static',
        keyboard: false
    });
}

function addClosingButton(modalId) {
    var closeButton = '<button class="btn btn-xs btn-danger pull-right" data-dismiss="modal">&times;</button>';
    $("#" + modalId + " .panel-heading").append(closeButton);
}


$(".data-filter-control").each(function () {
    if ($(this).attr("type") === "date" || $(this).is('select')) {
        $(this).change(function () {
            sendFilterRequest();
        });
    } else {
        $(this).keyup(function () {
            sendFilterRequest();
        });
    }

});

function sendFilterRequest() {
    var data = $("form[name=filter-form]").serialize();
    console.log(data);
    $.ajax({
        type: method, //this variable should be defined in the @section scripts section of the file/page where thid function is being called from
        url: action,  //this variable should be defined in the @section scripts section of the file/page where thid function is being called from
        data: data,
        datatype: "html",
        success: function (result) {
            $('#data-results').html(result);
            patientChangers();
            universalModalCaller();
            clickablePatientList();
        },
        error: function (e, x, s) {
            console.log(e.responseText);
        }
    });
}

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

$("form").each(function () {
    $(this).validate();
});

$("#workOrderForm").click(function () {
    $("#workOrderModal").modal("show");
});

window.onbeforeunload = function () {
    if (sessionStorage.hasUnsavedChanges) {
        return 'You have unsaved changes!\n\nAre you sure you want to leave?';
    }
    
};

function isValidForm(form) {
    var validity = true;
    $(form).find(":required").each(function () {
        if ($(this).val() == '') {
            validity = false;
        }
    });

    return validity;
}

function progressBar(progress, target, message) {
    if (typeof (message) === 'undefined') {
        message = "Please wait..."
    }
    target.html('<div class="progress">' +
        '<div class="progress-bar progress-bar-striped progress-bar-animated active" role= "progressbar" aria-valuenow="' + progress + '" aria-valuemin="0" aria-valuemax="100" style="width:' + progress + '%">' + message+'</div>' +
        '</div>');
}

$("tr.new-entry-row").each(function () {
    $(this).find('td').each(function () {
        if ($(this).hasClass("actions")) {
            //skip
        } else {
            $(this).attr('contenteditable', 'true');
        }
       
    });

});

$("img.img-link").each(function () {
    $(this).click(function () {
        var href = $(this).attr("data-href");
        if (typeof (href) !== "undefined") {
            window.location.href = href;
        }
    });
});

var pathName = window.location.pathname;

clickablePatientList();
function clickablePatientList() {
    $("tr.tr-href").each(function () {
        var tr = $(this);
            tr.find("td").each(function () {
            /*Exempt some td from this functionality*/
            if ($(this).hasClass("prevent-href-prop")) {
                //do nothing
            } else {
                $(this).click(function () {
                    var href = tr.attr("data-href");
                    if (typeof (href) !== "undefined") {
                        if (tr.hasClass("Unpaid") && tr.hasClass("lockable")) {
                            showNotification("This patient has not cleared with the cashiers!", "warning", true);
                        } else {
                            window.location.href = href;
                        }

                    }
                });
            }
        })
        
        //disable embebded links if unpaid and locakble
        if ($(this).hasClass("Unpaid") && $(this).hasClass("lockable")) {
            $(this).find("a").addClass("disabled");
            $(this).find("a").prop("href", "#");
            $(this).find("a").click(function () {
                showNotification("This patient has not cleared with the cashiers!", "warning", true);
            })
        }
    });

}


$("table.table-top-menu td").each(function () {
    if (typeof ($(this).attr("data-href")) !== "undefined") {
        if ($(this).attr("data-href") === pathName) {
            $(this).addClass("isActive");
        }
    }

    $(this).click(function () {
        if (typeof ($(this).attr("data-href")) !== "undefined") {
            if ($(this).hasClass("isActive")){
                //don't bother
            } else {
                window.location.href = ($(this).attr("data-href"));
            }
            
        }
    });
});


$("table.table-dashboard-icons td").each(function () {


    $(this).click(function () {
        var href = $(this).attr("data-href");
        if (typeof (href) !== "undefined") {
            window.location.href = href;
        }
    });
});
patientChangers();
function patientChangers() {
    $(".set-current-patient").each(function () {
        
        $(this).click(function () {
            localStorage.CurrrentPatient = $(this).attr("data-opd-no");
            var callBack = $(this).attr("data-callback");

            if (typeof (window[callBack]) === "function") {
                window[callBack]();
            } else {
                console.log("Function " + callBack + " not found");
            }
        });
    });
}


function Triage() {
    window.location.href = host + "/emr/physicalexamination/";
}

function Consultation() {
    window.location.href = host + "/emr/consultation/";
}

$(".change-current-patient").each(function () {
    $(this).click(function () {

    });
});

universalModalCaller();

function universalModalCaller() {
    $(".call-modal-form").each(function () {
        $(this).unbind();
        $(this).click(function (e) {
            e.preventDefault();
            $(".loader").show();
            //var OPDNo = $(this).data("opd");
            var action = $(this).data("action");
            //var url = "@Url.Action(" - 1", new {id =  -2})";
            var callback = $(this).data("callback");
            var fallback = $(this).data("fallback");
            //url = url.replace("-1", action).replace("-2", OPDNo);

            $.ajax({
                type: "GET",
                url: action/* + "/" + OPDNo*/,
                datatype: "html",
                success: function (result) {
                    if (typeof (result) === "object") {
                        $(".loader").hide();
                        showNotification(result.message, result.status, true);
                        if (result.status.toLowerCase() !== "success") {
                            if (typeof (fallback) !== "undefined") {
                                window[fallback]();
                            }
                        }
                            
                        return false;
                    }

                    $(".loader").hide();
                    $("#modal-content").html(result);
                    preventModalBackdrop("modal-forms-holder");
                    addClosingButton("modal-forms-holder");
                    $('#modal-forms-holder').modal('show');
                    if (typeof (callback) !== 'undefined') {
                        window[callback]();
                    }
                    
                },
                error: function (xhr, status) {
                    $(".loader").hide();
                }
            });
        });
    });
}



function showNotification(message, type, selfDismiss) {
    if (typeof (selfDismiss) === "undefined") {
        selfDismiss = false;
    }
    $("#notification-message").text(message);
    $("#notification").addClass("notification-" + type);
    $("#notification").fadeIn();
    if (selfDismiss) {
        var myTimeOut;
        myTimeOut = setTimeout(function () {
            $("#close-notification").trigger("click");
        }, 5000);

        $("#notification").mouseout(function () {
            myTimeOut = setTimeout(function () {
                $("#close-notification").trigger("click");
            }, 5000)
        });

        $("#notification").mouseover(function () {
            clearTimeout(myTimeOut);
        });
    }
}

function emphasise(selector, type, duration) {
    
   // $(selector).css({ "background": "red", "transition-duration": "3s" });
    //$(selector).css({ "background": "white", "transition-duration": "3s", "transition-delay":" 4s" });
    
}



$("#close-notification").click(function () {
    var notification = $(this).parent();
    notification.hide(function () {
        notification.removeClass();
    });
});

function BioMetricIdListener() {

}

markRequiredFields();
function markRequiredFields() {
    $("form input,textarea,select").each(function () {
        if ($(this).prop('required')) {
            var id = $(this).prop('id');
            if (!$(this).hasClass("asterisk")) {
                $("label[for=" + id + "]").append("<span class='text-danger'> *</span>");
                $(this).addClass("asterisk");
            }
            
        }
    });
}


moneyfy();

function moneyfy() {
    $(".money").each(function () {
        if (!$(this).hasClass("moneyfied")) {
            var amount = $(this).text().trim();
            if (amount == "") {
                amount = 0;
            }
            $(this).text(parseFloat(amount).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            $(this).addClass("moneyfied");

            if ($(this).prev().text().trim().toLowerCase() === "total") {
                $(this).css({ "color": "#009EE0" });
            }
        }
    });

}

cb_for_input_text_listener();
function cb_for_input_text_listener() {
    $(".cb_for_input_text").click(function () {

        var textbox = $(this).data("textbox");
        var value = $(this).data("value");

        if ($(this).is(":checked")) {
            $("#" + textbox).val(value);
        } else {
            $("#" + textbox).val("");
        }
    })
}



function colorCodeLatestPatientVitals() {
    var vitalRows = $("#latest-patient-vitals tr");
    var key = $("#vital-range-values tbody tr");
    $.each(vitalRows, function (i, v) {

        var vitalName = $(v).find("td").first().text().trim();
        var vitalValue = $(v).find("td").last().text().trim();

        $.each(key, function (i, t) {
            console.log($(t).text().trim());
        });
    })
}

function dischargeRecommendationFormListener() {
    $("form").submit(function (e) {
        $(".loader").show();

        e.preventDefault();

        var data = $(this).serializeObject();
        data.TimeToDischarge = data.date + " " + data.time;
        console.log(data);
        
        $.ajax({
            url: "/emr/SaveDischargeRecommendation",
            method: "POST",
            data: data,
            success: function () {
                $(".loader").hide();
                showNotification("Recommendation Saved Successfully!", "success", true);
                $('#modal-forms-holder').modal('hide');
                $("#btn_recommend_discharge").prop("disabled", true);
            },
            error: function (e, x, h) {
                $(".loader").hide();
                console.log(e.responseText)
            }
        })
    })
}

function applyHumanTime() {
    $(".human-time").each(function () {
        if (!$(this).hasClass("applied")) {
            var time = $(this).text().trim();
            if (time !== "Never") {
                var m = moment(time, "DD-MMM-YY h:mm:ss a").fromNow();
                $(this).html("<em><small>"+m+"</small></em>");
                $(this).addClass('applied');
            }
           
        }
    })
}

function validateTelFields() {
    $("input[type=tel]").each(function () {
        $(this).keyup(function () {
            var user_input = $(this).val();
            if (!$.isNumeric(user_input)) {
                user_input = user_input.slice(0, -1);
                $(this).val(user_input);
            }
        });
    })

}

function investigationModalListener() {
    var opdNo = $("#OPDNo").val();
    if (location.search === "?v=results") {
        $('a[href="#view-investigation-tab"]').tab('show');
    }
    $.ajax({
        url: "/Pathology/workorders/OPDLabResults/" + opdNo,
        success: function (res) {
            $("#my-lab-results").html(res);
        },
        error: function (e, x, h) {
            showNotification("Error retrieving lab results!", "danger", true);
            console.log(e.responseText);
        }
    })

    $("#SearchTest").autocomplete({
        source: function (req, res) {
            $.ajax({
                type: "POST",
                url: "/emr/SearchInvestigations/",
                data: { search: $("#SearchTest").val(), from: $("input[name=search-from]:checked").val(), id: opdNo },

                success: function (data) {
                    res($.map(data, function (item) {

                        return {
                            label: item.Name + " " + item.Available, value: item.Name, Id: item.Id, TestCode: item.TestCode,
                            AwardedAmount: item.AwardedAmount, Department: item.Department, PayableAmount: item.PayableAmount,
                            Price: item.Price, Available:item.Available
                        };
                    }));
                },
                error: function (xmlhttprequest, textstatus, errorthrown) {
                    console.log(errorthrown, "danger")
                    console.log(xmlhttprequest.responseText);
                }
            });
        }
    });



    $("#SearchTest").on("autocompleteselect", function (event, ui) {

        var ServiceId = ui.item.Id;
        var data = ui.item;
        console.log(ui.item);
        if ($("#draft-service-" + ServiceId).data("status") === "draft") {
            //the request is still in draft and cannot be resent just yet
            showNotification("You already requested this test!", "info", true);
            return false;
        } else {
            console.log($("#draft-service-" + ServiceId).data("status"));
        }

        var selectBox = '<select class="text-box" id="priority-' + ServiceId + '">' +
            '<option>Normal</option>' +
            '<option>Urgent</option>' +
            '<option>Emergency</option>' +
            '</select>';

        var tr = "<tr id='draft-service-" + ServiceId + "' data-status='draft' data-serviceid=" + ServiceId + "><td><label class='delete-row btn btn-xs'>Delete</label></td><td>" + data.TestCode +
            "</td><td>" + data.label + "</td><td>" + data.Price + "</td><td>" + data.AwardedAmount + "</td><td>" + (data.PayableAmount - data.AwardedAmount) + "</td><td id='service-" + ServiceId + "-department'>" + data.Department +
            "</td><td>Draft</td><td>" + selectBox + "</td><td></td></tr>";
        $("#table-selected-tests tbody").append(tr);

        $(".delete-row").each(function () {
            $(this).unbind();
            $(this).click(function () {
                $(this).parent().parent().remove();
            });
        })

        $.ajax({
            success: function () {
                $("#SearchTest").val('');
            }
        });


    });



    $("#btn-submit-workorder").click(function () {
        var workOrder = [];

        $("#table-selected-tests tbody tr").each(function () {
            if ($(this).data("status") === 'draft') {
                var serId = $(this).data("serviceid");
                var test = {
                    "ServiceId": serId,
                    "Department": $("#service-" + serId + "-department").text(),
                    "OrderPriority": $("#priority-" + serId).val()
                }

                workOrder.push(test);
            }
        })

        if (workOrder.length < 1) {
            showNotification("Nothing to send!", "warning", true);
            return false;
        }
        $(".loader").show();

        var data = {
            "OPDIPDNo": $("#OPDNo").val(),
            "DoctorNotes": $('#DoctorNotes').val(),
            "Tests": workOrder
        };

        console.log(data);

        $.ajax({
            url: '/emr/SendTestRequest',
            method: "post",
            data: data,
            success: function (result) {
                console.log(result);
                $(".loader").hide();
                showNotification("Test request sent successfully!", "success", true);
                $('#modal-forms-holder').modal('hide');

            },
            error: function (e, x, r) {
                $(".loader").hide();
                showNotification("An error occured!", "danger", true);
            }
        })
    })
}

function showNotificationFromViewBags() {
    var status = $("#status").val();
    var message = $("#message").val();
    if (status.trim() !== "") {
        showNotification(message, status, true);
    }
}



//Kogi Code

$("#SelectItemSearch").bind('change keyup', function () {
    var search = $(this).val();
    $('#SelectItem option').each(function () {

        if (($(this).text()).toLowerCase().indexOf(search) >= 0) {
            $(this).removeClass("collapse");

        } else {
            $(this).addClass("collapse");

        }
    });
});