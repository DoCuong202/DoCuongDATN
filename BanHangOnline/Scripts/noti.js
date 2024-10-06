$(document).ready(function () {
    $("#btnDeleteAll").hide();
    $('.btnUpdate').hide();
    $('body').on('change', '.input-update', function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        $('.btnUpdate').show();
    })

    $("body").on('click', ".btnUpdate", function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        var quantity = $("#Quantity_" + id).val();

        $.ajax({
            url: '/shoppingcart/Update',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.quantity <= 0) {
                    location.reload();
                }
                if (rs.success) {
                    $('.btnUpdate').hide();
                    /*                    $(".input-update").text(quantity);*/
                    location.reload();
                }
            }
        })
    })

    $('body').on('click', '.btnDelete', function () {
        var id = $(this).data("id");
        var conf = confirm("Bạn chắc chắn xóa");
        if (conf === true) {
            $.ajax({
                url: '/shoppingcart/delete',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.success) {
                        $('#trow_' + id).remove();
                        $("#checkout_items").html(rs.count);
                    }
                }
            })
        }
    });


    var Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
    });

    $('.swalDefaultSuccess').click(function () {

    });

    $("body").on('click', ".btnAddCart", function (e) {
        e.preventDefault();

        var id = $(this).data("id");
        var quatity = 1;
        var sl = parseInt($("#quantity_value").text());
        if (sl != "") {
            quantity = parseInt(sl);
        }

        $.ajax({
            url: '/shoppingcart/addtocart',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.Success) {
                    $("#checkout_items").val(rs.count);
                    Toast.fire({
                        icon: 'success',
                        title: 'Thêm thành công.'
                    })
                }
            }
        })

        ShowCount();
    })



    $('body').on('change', '#SelectAll', function () {
        var checkStatus = this.checked;
        var checkbox = $(this).parents('.allproducts').find('tr td input:checkbox');
        checkbox.each(function () {
            this.checked = checkStatus;
            if (this.checked) {
                checkbox.attr('selected', 'checked');
            } else {
                checkbox.attr('selected', '');
            }
        })
    })


    $('body').on('click', '#btnDeleteAll', function (e) {
        e.preventDefault();
        var str = "";
        var checkbox = $(this).parents('.allproducts').find('tr td input:checkbox');
        var i = 0;
        checkbox.each(function () {
            if (this.checked) {
                checkbox.attr('selected', 'checked');
                var id = $(this).val();
                if (i === 0) {
                    str += id;
                } else {
                    str += "," + id;
                }
                i++;
            } else {
                checkbox.attr('selected', '');
            }
        })
        if (str.length > 0) {
            var conf = confirm("Bạn có muốn xóa các bản ghi này hay không?");
            if (conf === true) {
                $.ajax({
                    url: '/shoppingcart/deleteAll',
                    type: 'POST',
                    data: { ids: str },
                    success: function (rs) {
                        if (rs.success) {
                            location.reload();
                        }
                    }
                })
            }
        }
    })

    //var Toast = Swal.mixin({
    //    toast: true,
    //    position: 'top-end',
    //    showConfirmButton: false,
    //    timer: 3000
    //});
    //$(".swalDefaultSuccess").click(function () {
    //    debugger;
    //    Toast.fire({
    //        icon: 'success',
    //        title: 'Đã thêm giỏ hàng.'
    //    })
    //});
})

function ShowCount() {
    $.ajax({

        url: '/shoppingcart/ShowCount',
        type: 'GET',
        success: function (rs) {
            if (rs.Success) {
                $("#checkout_items").html(rs.count);
            }
        }
    })
}