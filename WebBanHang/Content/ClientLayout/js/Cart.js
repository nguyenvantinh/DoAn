var cart = {
    init: function () {
        cart.registerEvents();
        cart.getProductCount();
    },
    registerEvents: function () {
        $('#DSAddCart, i#DSAddCart').off('click').on('click', function (e) {
            e.preventDefault();
            debugger
            var productId = parseInt($(this).data('id'));
            cart.addItem(productId);
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
        debugger
        $.ajax({
            url: '/GioHang/TinhSoLuongItemGioHang',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                        cart.setProductCount(res.Total);
                }
                else {
                    cart.setProductCount(res.Total);
                }
            }
        });
    },
    setProductCount: function (num) {
        $('span.no_product').text(num);
    }
}
cart.init();