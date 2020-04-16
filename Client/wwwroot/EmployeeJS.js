var table = null;

$(document).ready(function () {
    debugger;
    table = $('#Employee').DataTable({
        "processing": true,
        "ajax": {
            url: "/Employees/LoadEmployee",
            type: "GET",
            "datatype": "json",
            "dataSrc": "",
        },
        "columnDefs":
            [{
                "targets": [3],
                "orderable": false
            }],
        "columns": [
            { "data": "firstName" },
            { "data": "lastName" },
            { "data": "email" },
            {
                "data": "birthDate", "render": function (data) {
                    return moment(data).tz("Asia/Jakarta").format('MMMM Do YYYY');
                }
            },
            { "data": "phoneNumber" },
            { "data": "address" },
            { "data": "deptName" },
            {
                "data": "createDate", "render": function (data) {
                    return moment(data).tz("Asia/Jakarta").format('MMMM Do YYYY, h:mm:ss a');
                }
            },
            {
                "data": "updateDate", "render": function (data) {
                    var dateupdate = "Not Updated Yet";
                    //debugger;
                    if (data == null) {
                        return dateupdate;
                    } else {
                        return moment(data).tz("Asia/Jakarta").format('MMMM Do YYYY, h:mm:ss a');
                    }
                }
            },
            {
                "render": function (data, type, row) {
                    return '<button class="btn btn-warning " data-placement="left" data-toggle="tooltip" data-animation="false" title="Edit" onclick="return GetById(' + row.id + ')"> <i class="mdi mdi-pencil"></i></button >' + '&nbsp;' +
                        '<button class="btn btn-danger" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return Delete(' + row.id + ')"> <i class="mdi mdi-eraser"></i></button >'
                }
            }]
    });
});

function ClearScreen() {
    $('#Id').val('');
    $('#first').val('');
    $('#last').val('');
    $('#email').val('');
    $('#birthdate').val('');
    $('#phone').val('');
    $('#address').val('');
    $('#Department').val('');
    $('#Update').hide();
    $('#Save').show();
}

var Departments = []
function LoadDepartment(element) {
    //debugger;
    if (Departments.length == 0) {
        $.ajax({
            type: "Get",
            url: "/Departments/LoadDepartment",
            success: function (data) {
                //debugger;
                Departments = data.data;
                //Departments = data;
                renderDepartment(element);
            }
        })
    }
    else {
        renderDepartment(element);
    }
}

function renderDepartment(element) {
    //debugger;
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select Department').hide());
    $.each(Departments, function (i, val) {
    //$.each(Departments.data, function (i, val) {
        //debugger;
        $ele.append($('<option/>').val(val.id).text(val.name));
    })
}
LoadDepartment($('#Department'));

function Save() {
    if ($('#first').val() == 0) {
        Swal.fire({
            position: 'center',
            type: 'error',
            title: 'Please Full Fill The First Name',
            showConfirmButton: false,
            timer: 1500
        });
    }
    else if ($('#last').val() == 0) {
        Swal.fire({
            position: 'center',
            type: 'error',
            title: 'Please Full Fill The Last Name',
            showConfirmButton: false,
            timer: 1500
        });
    }
    else if ($('#email').val() == 0) {
        Swal.fire({
            position: 'center',
            type: 'error',
            title: 'Please Full Fill The Email',
            showConfirmButton: false,
            timer: 1500
        });
    }
    else if ($('#birthdate').val() == 0) {
        Swal.fire({
            position: 'center',
            type: 'error',
            title: 'Please Full Fill The Birthdate',
            showConfirmButton: false,
            timer: 1500
        });
    }
    else if ($('#phone').val() == 0) {
        Swal.fire({
            position: 'center',
            type: 'error',
            title: 'Please Full Fill Phone Number',
            showConfirmButton: false,
            timer: 1500
        });
    }
    else if ($('#address').val() == 0) {
        Swal.fire({
            position: 'center',
            type: 'error',
            title: 'Please Full Fill The Address',
            showConfirmButton: false,
            timer: 1500
        });
    }
    else {
        debugger;
        var Employee = new Object();
        Employee.firstName = $('#first').val();
        Employee.lastName = $('#last').val();
        Employee.email = $('#email').val();
        Employee.birthDate = $('#birthdate').val();
        Employee.phoneNumber = $('#phone').val();
        Employee.address = $('#address').val();
        Employee.deptId = $('#Department').val();
        $.ajax({
            type: 'POST',
            url: '/Employees/InsertOrUpdate/',
            data: Employee
        }).then((result) => {
            debugger;
            if (result.statusCode == 201 || result.statusCode == 204 || result.statusCode == 200) {
                Swal.fire({
                    position: 'center',
                    type: 'success',
                    title: 'Employee Added Successfully'
                });
                table.ajax.reload();
            } else {
                Swal.fire('Error', 'Failed to Input', 'error');
                ClearScreen();
            }
        })
    }
}


function GetById(id) {
    //debugger;
    $.ajax({
        url: "/Employees/GetById/",
        data: { id: id }
    }).then((result) => {
        debugger;
        if (result) {
            $('#Id').val(result.id);
            $('#first').val(result.firstName);
            $('#last').val(result.lastName);
            $('#email').val(result.email);
            $('#birthdate').val(result.birthDate);
            $('#phone').val(result.phoneNumber);
            $('#address').val(result.address);
            $('#Department').val(result.department_Id);
            $('#myModal').modal('show');
            $('#Update').show();
            $('#Save').hide();
        }
    })
}

function Update() {
    debugger;
    var Employee = new Object();
    Employee.id = $('#Id').val();
    Employee.firstName = $('#first').val();
    Employee.lastName = $('#last').val();
    Employee.email = $('#email').val();
    Employee.birthDate = $('#birthdate').val();
    Employee.phoneNumber = $('#phone').val();
    Employee.address = $('#address').val();
    Employee.deptId = $('#Department').val();
    $.ajax({
        type: "POST",
        url: '/Employees/InsertOrUpdate/',
        data: Employee
    }).then((result) => {
        debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Employee Updated Successfully'
            });
            table.ajax.reload();
        } else {
            Swal.fire('Error', 'Failed to Update', 'error');
            ClearScreen();
        }
    })
}

function Delete(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            debugger;
            $.ajax({
                url: "/Employees/Delete/",
                data: { id: id }
            }).then((result) => {
                debugger;
                if (result.statusCode == 200) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Delete Successfully'
                    });
                    table.ajax.reload();
                } else {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    ClearScreen();
                }
            })
        };
    });
}