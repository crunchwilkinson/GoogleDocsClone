// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

toastr.options = {
    "positionClass": "toast-bottom-right",
    "progressBar": true,
    "timeOut": "3000"
};

document.addEventListener("DOMContentLoaded", function () {
    const deleteForm = document.getElementById('delete-form');

    if (deleteForm) {
        deleteForm.addEventListener('submit', function (e) {
            // 1. Stop the form from submitting immediately
            e.preventDefault();

            // 2. Trigger the modern modal
            Swal.fire({
                title: 'Delete Document?',
                text: "This action cannot be undone!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33', // Google-style red
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'Keep it',
                background: '#fff',
                customClass: {
                    popup: 'format-modal' // You can style this in CSS
                }
            }).then((result) => {
                // 3. If "Yes" was clicked, manually submit
                if (result.isConfirmed) {
                    deleteForm.submit();
                }
            });
        });
    }
});