var cart = {
    init: function () {
        cart.registerEvents();
        cart.getProductCount();
    },
    registerEvents: function () {
        $('.DSAddCart').off('click').on('click', function (e) {
            e.preventDefault();
            debugger
            var productId = parseInt($(this).data('id'));
            cart.addItem(productId);
        });
        $(".update").off('click').on('click', function (e) {
            e.preventDefault();
            debugger
            var productId = parseInt($(this).data('id'));
            var a = "#soluong" + $(this).data('soluong');
            var soluong = parseInt($(a).val());
            cart.UpDate(productId, soluong);
        });
        $(".delete").off('click').on('click', function (e) {
            e.preventDefault();
            debugger
            var productId = parseInt($(this).data('id'));
            cart.Delete(productId);
        });
    },
    Delete: function (productId) {
        $.ajax({
            url: '/GioHang/XoaItemGioHang',
            type: 'POST',
            dataType: 'json',
            data: {
                maSP: productId
            },
            success: function (res) {
                if (res.status) {
                    $('#row_' + productId).remove();
                    $.notify("Đã xóa", "success");
                    cart.getProductCount();
                    console.log($('span.no_product').text());
                    var tbody = $("#tableGH tbody");

                    if (tbody.children().length == 0) {
                        window.location.href = "/GioHang/XemGioHang"
                    }
                    
                }
                else {
                }
            }
        });
    },

    UpDate: function (productId, soluong) {
        $.ajax({
            url: '/GioHang/SuaGioHang',
            type: 'POST',
            dataType: 'json',
            data: {
                maSP: productId,
                soluong: soluong
            },
            success: function (res) {
                if (res.status) {
                    cart.getProductCount();
                    //toastr.success('Thêm vào giỏ thành công.');
                    $.notify("Đã cập nhật", "success")
                    setTimeout(function () {
                        window.location.href = "/GioHang/XemGioHang";
                    }, 3000);
                }
                else {
                    $.notify(res.mes, "error")
                    setTimeout(function () {
                        window.location.href = "/GioHang/XemGioHang";
                    }, 1000);
                   
                }
            }
        });
    },

    addItem: function (productId) {
        $.ajax({
            url: '/GioHang/ThemItemGioHang',
            type: 'POST',
            dataType: 'json',
            data: {
                maSP: productId
            },
            success: function (res) {
                if (res.status) {
                    cart.getProductCount();
                    //toastr.success('Thêm vào giỏ thành công.');
                    $.notify("Thêm giỏ hàng thành công", "success")
                }
                else {
                    //setTimeout(function () {
                    //    toastr.error(res.message);
                    //}, 1800);
                    $.notify(res.mes, "error")
                }
            }
        });
    },

    getProductCount: function () {
        $.ajax({
            url: '/GioHang/TinhSoLuongItemGioHang',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    cart.setProductCount(res.Total);
                }
            }
        });
    },
    setProductCount: function (num) {
        $('span.no_product').text("(" + num + " sản phẩm)");
    }
}
cart.init();