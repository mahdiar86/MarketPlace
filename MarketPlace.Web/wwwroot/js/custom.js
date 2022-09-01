function open_waiting(selector = 'body') {
    $(selector).waitMe({
        effect: 'bounce',
        text: 'لطفا صبر کنید ...',
        bg: 'rgba(255,255,255,0.7)',
        color: '#000'
    });
    console.log("blob blob");
}

function close_waiting(selector = 'body') {
    $(selector).waitMe('hide');
}


$("[category_checkbox]").on('change',
    function (e) {
        var isChecked = $(this).is(":checked");
        var selectedCategoryId = $(this).attr("category_checkbox");
        if (isChecked === true) {
            $("#sub_categories_" + selectedCategoryId).slideDown(300);
        } else {
            var subCategories = $("[parent-category-id='" + selectedCategoryId + "']").prop('checked', false);
            $("#sub_categories_" + selectedCategoryId).slideUp(300);
        }
    });


$(document).ready(function () {
    var editors = $("[ckeditor]");
    if (editors.length > 0) {
        $.getScript('/js/ckeditor.js', function () {
            $(editors).each(function (index, value) {
                var id = $(value).attr('ckeditor');
                ClassicEditor.create(document.querySelector('[ckeditor="' + id + '"]'),
                    {
                        toolbar: {
                            items: [
                                'heading',
                                '|',
                                'bold',
                                'italic',
                                'link',
                                '|',
                                'fontSize',
                                'fontColor',
                                '|',
                                'imageUpload',
                                'blockQuote',
                                'insertTable',
                                'undo',
                                'redo',
                                'codeBlock'
                            ]
                        },
                        language: 'fa',
                        table: {
                            contentToolbar: [
                                'tableColumn',
                                'tableRow',
                                'mergeTableCells'
                            ]
                        },
                        licenseKey: '',
                        simpleUpload: {
                            // The URL that the images are uploaded to.
                            uploadUrl: '/Uploader/UploadImage'
                        }

                    })
                    .then(editor => {
                        window.editor = editor;
                    }).catch(err => {
                        console.error(err);
                    });
            });
        });
    }
});

function FillPageId(pageId) {
    $("#PageId").val(pageId);
    $("#filter-form").submit();
}

function deleteProduct() {
    var productId = $("#modalProductId").val();


    if (productId != 0) {
        $.ajax({
            url: "/Seller/DeleteProduct",
            type: "post",
            data: { productId },
            success: function (response) {
                toast("محصول مورد نظر با موفقیت حذف شد", "info");
                $('#exampleModal').modal('hide');
                setTimeout(function () {
                    window.location.reload();
                },
                    1000);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                toast("خطایی در  حذف محصول رخ داده است", "warning");
            }
        });
    }
}

function setProductIdToModal(productId) {
    $("#modalProductId").val(productId);
}

function addGallery() {
    var productId = $("#GalleryProductId").val();
    var displayPriority = $("#GalleryDisplayPriority").val();
    var imageName = $("#GalleryImageName")[0].files[0];

    var fd = new FormData();
    fd.append("Gallery.ProductId", productId);
    fd.append("Gallery.DisplayPriority", displayPriority);
    fd.append("imageName", imageName);

    $.ajax({
        url: "/Seller/Create-Product-Gallery",
        type: "POST",
        data: fd,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.status === "Success") {
                console.log(response);
                toast(response.message, "success");
                $('#exampleModal').modal('hide');
                $("#GalleryProductId").val("");
                $("#GalleryDisplayPriority").val("");
                $("#GalleryImageName").val(null);
            } else {
                console.log(response);
                toast(response.message, response.status.toLowerCase());
            }
        },
        error: function (response) {
            toast("! خطایی رخ داده است", "warning");
        }
    });
}

function onSuccessAddProductToOrder(res) {
    setTimeout(function () {
        close_waiting();
        toast(res.message, res.status.toLocaleLowerCase());
    }, 800);

    console.log(res);
}

$(function () {
    $("#number_of_products_in_basket").on('change', function (e) {
        var numberOfProducts = parseInt(e.target.value, 0);
        $("#add_product_to_order_Count").val(numberOfProducts);
    });
});


$('#submit_order_form').on('click', function () {
    $('#addProductToOrderForm').submit();
    open_waiting();
});

function removeProductFromOrder(detailId) {
    $.get('/user/remove-order-item/' + detailId).then(res => {
        //$("[line-order-id='" + detailId + "']").hide('slow');
        location.reload();
    });
}

function changeOpenOrderCount(event, detailId) {
    open_waiting();
    $.get('/user/change-open-order/' + detailId + "/" + event.target.value).then(res => {

        $("#main-order-wrapper").html(res);

        close_waiting();

    }).catch(function (error) {
        console.log(error);
        close_waiting();
    });
}