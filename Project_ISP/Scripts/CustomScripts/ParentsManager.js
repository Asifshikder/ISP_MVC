var ParentsManager = {

    Validation: function () {

        if (AppUtil.GetIdValue("lstCourse") === '') {
            alert("Please Select Student Course.");
            return false;
        }
        if (AppUtil.GetIdValue("lstBatch") === '') {
            alert("Please Select Student Batch.");
            return false;
        }
        if (AppUtil.GetIdValue("StudentID") === '') {
            alert("Please Select Student Name.");
            return false;
        }

        if (AppUtil.GetIdValue("Name") === '') {
            alert("Please enter Parents Name.");
            return false;
        }
        if (AppUtil.GetIdValue("Mobile") === '') {
            alert("Please enter Parents Mobile.");
            return false;
        }
        if (AppUtil.GetIdValue("Job") === '') {
            alert("Please enter Parents Job.");
            return false;
        }

        if (AppUtil.GetIdValue("ParentsTypeID") === '') {
            alert("Please Select Parents Type.");
            return false;
        }
        return true;
    },

    SaveParentsForStudent: function () {

        var url = "/Parents/SaveParent/";

        var CourseID = $("#lstCourse").val();
        var BatchID = $("#lstCourse").val();
        var StudentID = $("#StudentID").val();
        var ParentsTypeID = $("#ParentsTypeID").val();
        var Name = $("#Name").val();
        var Mobile = $("#Mobile").val();
        var Job = $("#Job").val();
        
        var Parents = ({ StudentID: StudentID, BatchID: BatchID, StudentID: StudentID, ParentsTypeID: ParentsTypeID, Name: Name, Mobile: Mobile, Job: Job });
        var data = JSON.stringify({ parents: Parents, CourseID: CourseID, BatchID: BatchID });

        AppUtil.MakeAjaxCall(url, "POST", data, ParentsManager.SaveParentsForStudentSuccess, ParentsManager.SaveParentsForStudentError);


    },
    SaveParentsForStudentSuccess: function (data) {
        

        if (data.SaveStatus === false) {
            alert("Sorry Data Can Not Save.");
        }
        if (data.SaveStatus === true) {
            alert("Data Save Successfully.");
        }

        if (data.Exist == true) {
            if (data.ParentType == 1) {
                alert("Sorry Father Information Alreay Save For This Student.");
            }
            if (data.ParentType == 2) {
                alert("Sorry Mother Information Alreay Save For This Student.");
            }
        }
        if (data.Exist === false) {
            var parent = JSON.parse(data.Parent);
            if (parent) {
                
                $("#tblParents tbody").append('<tr><td style="padding:0px"><input type="hidden" id="ParentsID" value=' + parent.ParentsID + '></td><td>' + data.Course + '</td><td>' + data.Batch + '</td><td>' + parent.Student.FirstName + '</td><td>' + parent.Name + '</td><td>' + parent.ParentsType.ParentsTypeName + '</td><td>' + parent.Mobile + '</td><td>' + parent.Job + '</td><td>' + ' <div style="width: 50%; float: left"><button type="button" id="btnEdit" class="btn btn-success btn-block" style="width: 20px; padding: 0px 0px;"><span class="glyphicon glyphicon-ok"></span></button></div><div style="width: 50%; float: left"><button type="button" id="btnDelete" class="btn btn-danger btn-block" style="width: 20px; padding: 0px 0px;"><span class="glyphicon glyphicon-remove"></span></button></div>' + '</td></tr>');

            }
            ParentsManager.ClearInformation();
        }
    },
    SaveParentsForStudentError: function (data) {
        alert("fail");
        console.log(data);
    },

    GetBatchByCourseID: function (CourseID, AntiforgeryToken) {

        var url = "/Parents/GetBatchByCourseID/";
        var data = ({ __RequestVerificationToken: AntiforgeryToken, CourseID: CourseID });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ParentsManager.GetBatchByCourseIDSuccess, ParentsManager.GetBatchByCourseIDFailed);
    },
    GetBatchByCourseIDSuccess: function (data) {
        
        console.log(data);
        $.each(JSON.parse(data.BatchList), function (index, item) {
            $("#lstBatch").append($("<option></option>").val(item.BatchID).text(item.BatchName));
            //$.each(data, function (index, itemData) {
            //    $("#BatchID").append($('<option></option>').val(itemData.BatchID).html(itemData.BatchName))
            //});
        });

        //var EllectiveSubject = JSON.parse(data.Guardian);
        //ParentsManager.LoadEllectiveSubjectAfterDDLChange(EllectiveSubject);
    },
    GetBatchByCourseIDFailed: function (data) {
        
        alert("Fail");
        console.log(data);
    },

    GetStudentByBatchAndCourseID: function (courseID, batchID, AntiforgeryToken) {

        var url = "/Parents/GetStudentListByBatchAndCourseID/";
        var data = ({ __RequestVerificationToken: AntiforgeryToken, CourseID: courseID, BatchID: batchID });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ParentsManager.GetStudentByBatchAndCourseIDSuccess, ParentsManager.GetStudentByBatchAndCourseIDFailed);
    },
    GetStudentByBatchAndCourseIDSuccess: function (data) {
        
        console.log(data);
        $("#StudentID").find("option").not(":first").remove();
        $.each(JSON.parse(data.lstStudent), function (index, item) {
            $("#StudentID").append($("<option></option>").val(item.StudentID).text(item.FirstName));
            //$.each(data, function (index, itemData) {
            //    $("#BatchID").append($('<option></option>').val(itemData.BatchID).html(itemData.BatchName))
            //});
        });


    },
    GetStudentByBatchAndCourseIDFailed: function () {
        alert("Fail");
    },

    DeleteParents: function (ParentsIDForDelete) {
        // var AntiforgeryToken = $('input[name="__RequestVerificationToken"]').val();
        var url = "/Parents/DeleteParents/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ ParentsID: ParentsIDForDelete, __RequestVerificationToken: AntiForgeryToken });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ParentsManager.DeleteParentsSuccess, ParentsManager.DeleteParentsError);
    },
    DeleteParentsSuccess: function (data) {
        //   alert("Success");
        console.log(data);
        
        if (data.InfoNotFound == true) {
            alert("Sorry No Parents found for Remove.");
        }

        if (data.success == true) {
            alert("Parents Removed Success.");

            $("#tblParents tbody>tr").each(function () {
                var id = $(this).closest("tr").find("td:eq(0) input").val();
                console.log("id: " + id);
                if (id == data.DeleteParentsID) {
                    var index = $(this).index();
                    $('#tblParents tbody>tr:eq(' + index + ')').remove();
                }
            });

        }
        else if (data.success == false) {
            alert("Parents Removed Failed.");
        }

    },
    DeleteParentsError: function (data) {
        alert("failed");
        console.log(data);
    },

    LoadParentsInformationForEdit: function (ParentsID) {
        var AntiforgeryToken = $('input[name="__RequestVerificationToken"]').val();
        var url = "/Parents/LoadParentsForEdit/";
        var data = ({ __RequestVerificationToken: AntiforgeryToken, ParentsID: ParentsID });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ParentsManager.LoadParentsInformationForEditSuccess, ParentsManager.LoadParentsInformationForEditError);
    },
    LoadParentsInformationForEditSuccess: function (data) {
        
        console.log(data);

        if (data.Status === true) {
            var info = JSON.parse(data.ParentsInfo);
            var course = JSON.parse(data.CourseInfo);
            var batch = JSON.parse(data.BatchInfo);
            var student = JSON.parse(data.StudentInfo);

            $("#lstCourse").find('option').not(":first").remove();
            $("#lstBatch").find('option').not(":first").remove();
            $("#StudentID").find('option').not(":first").remove();

            $("#lstCourse").append($("<option></option>").val(course.CourseID).text(course.CourseName));
            $("#lstBatch").append($("<option></option>").val(batch.BatchID).text(batch.BatchName));
            $("#StudentID").append($("<option></option>").val(student.StudentID).text(student.FirstName));

            if (course.CourseID) {
                $("#lstCourse").prop("selectedIndex", 1);
            }
            if (batch.BatchID) {
                $("#lstBatch").prop("selectedIndex", 1);
            }
            if (student.StudentID) {
                $("#StudentID").prop("selectedIndex", 1);
            }
            $("#Name").val(info.Name);
            $("#Mobile").val(info.Mobile);
            $("#Job").val(info.Job);

            if (info.ParentsType.ParentsTypeName == "Father") {
                $("#ParentsTypeID").prop("selectedIndex", 1);
            }
            if (info.ParentsType.ParentsTypeName == "Mother") {
                $("#ParentsTypeID").prop("selectedIndex", 2);
            }

            //$.each(info, function (index,item) {

            //    //$("#tblParents tbody>tr").each(function () {
            //    //    
            //    //    var parentsID = $(this).find("td:eq(0) input").val();
            //    //    if (info.ParentsID == parentsID)
            //    //    {

            //    //    }
            //    //});
            //});
        }
        if (data.Status === false) {
            alert("Sorry No Parents Information For this Student.");
        }
    },
    LoadParentsInformationForEditError: function (data) {
        
        console.log(data);
    },

    UpdateParents: function () {
        var url = "/Parents/UpdateParent/";

        var CourseID = $("#lstCourse").val();
        var BatchID = $("#lstCourse").val();
        var StudentID = $("#StudentID").val();
        var ParentsTypeID = $("#ParentsTypeID").val();
        var Name = $("#Name").val();
        var Mobile = $("#Mobile").val();
        var Job = $("#Job").val();
        
        var Parents = ({ ParentsID: _UpdateParentsID, StudentID: StudentID, BatchID: BatchID, StudentID: StudentID, ParentsTypeID: ParentsTypeID, Name: Name, Mobile: Mobile, Job: Job });
        var data = JSON.stringify({ parents: Parents, CourseID: CourseID, BatchID: BatchID, UpdateParentsID: _UpdateParentsID });

        AppUtil.MakeAjaxCall(url, "POST", data, ParentsManager.UpdateParentsSuccess, ParentsManager.UpdateParentsError);
    },
    UpdateParentsSuccess: function (data) {
        
        console.log(data);

        if (data.ChangeParentsTypeExits === true) {
            if (data.ParentsTypes == 1) {
                alert("Sorry Father Information is Already Saved For this Batch.");
            }
            if (data.ParentsTypes == 2) {
                alert("Sorry Mother Information is Already Saved For this Batch.");
            }
            ParentsManager.ClearAllInformation();
            ParentsManager.GetAllCourseList();
            _UpdateMode = false;
        }

        if (data.UpdateStatus === true) {

            var ParentInformation = JSON.parse(data.UpdateParentInfo);

            $.each(ParentInformation, function (index, item) {

                $("#tblParents tbody>tr").each(function () {
                    
                    var parentsID = $(this).find("td:eq(0) input").val();
                    if (ParentInformation.ParentsID == parentsID) {
                        
                        var index = $(this).index();
                        $('#tblParents tbody>tr:eq(' + index + ')').find("td:eq(4)").text(ParentInformation.Name);
                        $('#tblParents tbody>tr:eq(' + index + ')').find("td:eq(5)").text(ParentInformation.ParentsType.ParentsTypeName);
                        $('#tblParents tbody>tr:eq(' + index + ')').find("td:eq(6)").text(ParentInformation.Mobile);
                        $('#tblParents tbody>tr:eq(' + index + ')').find("td:eq(7)").text(ParentInformation.Job);
                    }
                });
            });
            ParentsManager.ClearAllInformation()
            _UpdateMode = false;
            ParentsManager.GetAllCourseList();
        }
    },
    UpdateParentsError: function (data) {
        
        alert("Error Found Contack WIth Administrator.");
        console.log(data);
    },

    GetAllCourseList: function () {
        
        var url = "/Parents/GetAllCourseByAjax/";
        var anti = $("input[name='__RequestVerificationToken']").val();
        var data = ({ __RequestVerificationToken: anti });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ParentsManager.GetAllCourseSuccess, ParentsManager.GetAllCourseError);
    },
    GetAllCourseSuccess: function (data) {
        
        console.log(data);
        //  alert("Successs");
        var dta = JSON.parse(data.CourseList);

        $.each(dta, function (index, item) {
            
            $("#lstCourse").append($("<option></option>").val(item.CourseID).text(item.Course.CourseName));

        });

    },
    GetAllCourseError: function (data) {
        
        alert("Course List Retrieve error.");
    },

    ClearInformation: function () {
        $("#lstCourse").prop("selectedIndex", 0);
        $("#lstBatch").find('option').not(":first").remove();
        $("#StudentID").find('option').not(":first").remove();
        $("#Name").val("");
        $("#Mobile").val("");
        $("#Job").val("");
        $("#ParentsTypeID").prop("selectedIndex", 0);
    },

    ClearAllInformation: function () {
        $("#lstCourse").find('option').not(":first").remove();
        $("#lstBatch").find('option').not(":first").remove();
        $("#StudentID").find('option').not(":first").remove();
        $("#Name").val("");
        $("#Mobile").val("");
        $("#Job").val("");
        $("#lstCourse").prop("selectedIndex", 0);
        $("#lstBatch").prop("selectedIndex", 0);
        $("#StudentID").prop("selectedIndex", 0);
        $("#ParentsTypeID").prop("selectedIndex", 0);
    }
}