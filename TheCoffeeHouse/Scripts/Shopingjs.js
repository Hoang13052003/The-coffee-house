$(document).ready(function () {
    ShowCount();

    let basePrice = parseInt($("#total-price").text().replace(/\D/g, "")) || 0;

    let selectedSize = null;

    let selectedToppings = [];
    let totalToppingAndSize = 0;

    function updateTotalPrice() {
        totalToppingAndSize = (selectedSize ? selectedSize.price : 0);
        selectedToppings.forEach(topping => {
            totalToppingAndSize += topping.price;
        });

        let total = basePrice + totalToppingAndSize;
        $("#total-price").text(total.toLocaleString() + " đ");
    }

    //$(".size-btn").click(function () {
    //    $(".size-btn").removeClass("selected");
    //    $(this).addClass("selected");

    //    selectedSize = {
    //        name: $(this).text().trim(),
    //        price: parseInt($(this).data("price"))
    //    };

    //    updateTotalPrice();
    //});

    //$(".topping-btn").click(function () {
    //    let toppingName = $(this).text().trim();
    //    let toppingPrice = parseInt($(this).data("price"));

    //    let index = selectedToppings.findIndex(t => t.name === toppingName);
    //    if (index > -1) {
    //        selectedToppings.splice(index, 1);
    //        $(this).removeClass("selected");
    //    } else {
    //        selectedToppings.push({ name: toppingName, price: toppingPrice });
    //        $(this).addClass("selected");
    //    }

    //    updateTotalPrice();
    //});
    $('.size-btn').on('click', function () {
        $(".size-btn").removeClass("selected btn-warning"); // Reset màu
        $(this).addClass("selected btn-warning"); // Đổi màu khi được chọn

        selectedSize = {
            name: $(this).text().trim(),
            price: parseInt($(this).data("price")) || 0
        };

        updateTotalPrice();
    });

    $('.topping-btn').on('click', function () {
        let toppingName = $(this).text().trim();
        let toppingPrice = parseInt($(this).data("price"));

        let index = selectedToppings.findIndex(t => t.name === toppingName);
        if (index > -1) {
            selectedToppings.splice(index, 1);
            $(this).removeClass("selected btn-warning"); // Trở về màu ban đầu
        } else {
            selectedToppings.push({ name: toppingName, price: toppingPrice });
            $(this).addClass("selected btn-warning"); // Đổi màu khi chọn
        }

        updateTotalPrice();
    });

    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();

        if (!selectedSize || !selectedSize.name || selectedSize.name.trim() === "") {
            alert("Vui lòng chọn kích thước trước khi thêm vào giỏ hàng!");
            return;
        }

        let productId = $(this).data("id");
        let quantity = parseInt($("#quantity_value").text());
        let toppingNotes = selectedToppings.map(t => t.name).join(", ");
        $.ajax({
            url: '/ShoppingCart/AddToCart',
            type: 'POST',
            data: {
                id: productId,
                quantity: quantity,
                size: selectedSize.name,
                toppings: toppingNotes,
                extraPrice: totalToppingAndSize
            },
            success: function (rs) {
                if (rs.Success) {
                    $('#count-cart').html(rs.Count);
                    alert("Đã thêm vào giỏ hàng!\nTopping: " + toppingNotes + "Tổng giá topping: " + totalToppingAndSize);
                }
            }
        });
    });

    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var conf = confirm('Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?');
        if (conf == true) {
            $.ajax({
                url: '/ShoppingCart/Delete',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.Success) {
                        $('#count-cart').html(rs.Count);
                        $('#product_' + id).remove();
                        location.reload();
                    }
                }
            });
        }
    });
    $('body').on('click', '.deleteAll', function (e) {
        e.preventDefault();
        var conf = confirm('Bạn có chắc muốn xóa tất cả sản phẩm khỏi giỏ hàng?');
        if (conf == true) {
            $.ajax({
                url: '/ShoppingCart/DeleteAll',
                type: 'POST',
                success: function (rs) {
                    if (rs.Success) {
                        $('#count-cart').html(rs.Count);
                        $('#productCart').remove();
                        location.reload();
                    }
                }
            });
        }
    });

});

function ShowCount() {
    $.ajax({
        url: '/ShoppingCart/ShowCount',
        type: 'GET',
        success: function (rs) {
            $('#count-cart').html(rs.Count);
        }
    });
}

