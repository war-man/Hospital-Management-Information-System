var symptoms = "";

$(".symptoms").click(function () {

symptoms = "";


$('.symptoms:checkbox:checked').each(function (x, y) {

    symptoms += $(this).val() + ", ";
})

$("#symptoms").val(symptoms);
$("#nutritionform").submit();

    //alert(symptopms);
});

function dietChatCallback(opd) {

    $("#submitDietChart").click(function () {
        //alert();
    });

}

function dietSchedulerCallback(opd) {
    $("#OPDIPDID").val(opd);

    $("#submitDietScheduller").click(function () {
        //alert();
    });

}
