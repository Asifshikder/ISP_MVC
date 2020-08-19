var AssignSubjectManager = {
    Validation: function () {

        if (AppUtil.GetIdValue("lstCourse") === '') {
            alert("Please Select Course.");
            return false;
        }
        if (AppUtil.GetIdValue("lstBatch") === '') {
            alert("Please Select Batch.");
            return false;
        }
        if (AppUtil.GetIdValue("lstSubject") === '') {
            alert("Please Select Subject.");
            return false;
        }
        return true;

    },

    GetBatchByCourseID: function (CourseID, AntiforgeryToken) {

        var url = "/AssignSubject/GetBatchByCourseID/";
        var data = ({ __RequestVerificationToken: AntiforgeryToken, CourseID: CourseID });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AssignSubjectManager.GetBatchByCourseIDSuccess, AssignSubjectManager.GetBatchByCourseIDFailed);
    },
    GetBatchByCourseIDSuccess: function (data) {
        
        console.log(data);
        $.each(JSON.parse(data.BatchList), function (index, item) {
            $("#lstBatch").append($("<option></option>").val(item.BatchID).text(item.BatchName));

        });

        var resultAssignSubject = JSON.parse(data.listAssignSubject);

        AssignSubjectManager.LoadSubjectAllocationAfterDDLChange(resultAssignSubject);
    },
    GetBatchByCourseIDFailed: function (data) {
        alert("Fail");
        console.log(data);
    },


    LoadSubjectAllocationAfterDDLChange: function (resultAssignSubject) {
        $("#tblAssignSubject tbody>tr").empty();
        $.each(resultAssignSubject, function (index, itemData) {
            
            console.log(itemData);
            //  .SubjectAllocation.Course.CourseName + '</td><td>' + itemData.SubjectAllocation.Batch.BatchName + '</td><td>' + itemData.SubjectAllocation
            $("#tblAssignSubject tbody").append('<tr><td style="padding:0px"><input id="item_AssignSubjectID" type="hidden" value=' + itemData.AssignSubjectID + '></td><td style="padding:0px"><input id="item_SubjectAllocation_CourseID" type="hidden" value=' + itemData.SubjectAllocation.CourseID + '></td><td style="padding:0px"><input id="item_SubjectAllocation_BatchID" type="hidden" value=' + itemData.SubjectAllocation.BatchID + '></td><td style="padding:0px"><input id="item_SubjectAllocationID" type="hidden" value=' + itemData.SubjectAllocationID + '></td><td>' + itemData.SubjectAllocation.Course.CourseName + '</td><td>' + itemData.SubjectAllocation.Batch.BatchName + '</td><td>' + itemData.SubjectAllocation.Subject.SubjectName + '</td><td>' + ' <div style="width: 30%; float: left"><button type="button" id="btnEdit" class="btn btn-success btn-block" style="width: 40px;"><span class="glyphicon glyphicon-ok"></span></button></div><div style="width: 30%; float: left"><button type="button" id="btnDelete" class="btn btn-danger btn-block" style="width: 40px;"><span class="glyphicon glyphicon-remove"></span></button></div>' + '</td></tr>');
        });
    },

    GetSubjectByBatchAndCourseID: function (courseID, batchID, AntiforgeryToken) {

        var url = "/AssignSubject/GetSubjectListByBatchAndCourseID/";
        var data = ({ __RequestVerificationToken: AntiforgeryToken, CourseID: courseID, BatchID: batchID });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AssignSubjectManager.GetSubjectByBatchAndCourseIDSuccess, AssignSubjectManager.GetSubjectByBatchAndCourseIDFailed);
    },
    GetSubjectByBatchAndCourseIDSuccess: function (data) {
        
        console.log(data);
        $.each(JSON.parse(data.SubjectAllocation), function (index, item) {
            $("#lstSubject").append($("<option></option>").val(item.SubjectAllocationID).text(item.Subject.SubjectName));
            //$.each(data, function (index, itemData) {
            //    $("#BatchID").append($('<option></option>').val(itemData.BatchID).html(itemData.BatchName))
            //});
        });

        var resultAssignSubject = JSON.parse(data.listAssignSubject);

        AssignSubjectManager.LoadSubjectAllocationAfterDDLChange(resultAssignSubject);
    },
    GetSubjectByBatchAndCourseIDFailed: function () {
        alert("Fail");
    },

    GetAssignSubjectByBatchAndCourseAndSubjectID: function (subjectID, AntiforgeryToken) {
        //we dont need to take course and batch cause subject is working as allocation  and we can retrieve info from assignsubject by subjectallocationID
        var url = "/AssignSubject/GetAssignSubjectByBatchAndCourseAndSubjectID/";
        var data = ({ __RequestVerificationToken: AntiforgeryToken, SubjectID: subjectID });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AssignSubjectManager.GetAssignSubjectByBatchAndCourseAndSubjectIDSuccess, AssignSubjectManager.GetAssignSubjectByBatchAndCourseAndSubjectIDFailed);
    },
    GetAssignSubjectByBatchAndCourseAndSubjectIDSuccess: function (data) {
        
        console.log(data);

        var resultAssignSubject = JSON.parse(data.listAssignSubject);

        AssignSubjectManager.LoadSubjectAllocationAfterDDLChange(resultAssignSubject);
        
    },
    GetAssignSubjectByBatchAndCourseAndSubjectIDFailed: function () {
        alert("Fail");
    },

    GetAllAssignSubjectAfterSave: function () {
        //we dont need to take course and batch cause subject is working as allocation  and we can retrieve info from assignsubject by subjectallocationID

        var AntiforgeryToken = $("input[name='__RequestVerificationToken']").val();
        var url = "/AssignSubject/GetAllAssignSubjectAfterSave/";
        var data = ({ __RequestVerificationToken: AntiforgeryToken });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AssignSubjectManager.GetAllAssignSubjectAfterSaveSuccess, AssignSubjectManager.GetAllAssignSubjectAfterSaveFailed);
    },
    GetAllAssignSubjectAfterSaveSuccess: function (data) {
        
        console.log(data);

        var resultAssignSubject = JSON.parse(data.listAssignSubject);

        AssignSubjectManager.LoadSubjectAllocationAfterDDLChange(resultAssignSubject);
    },
    GetAllAssignSubjectAfterSaveFailed: function () {
        alert("Fail");
    },

    InsertAssignSubject: function (courseID, batchID, subjectID, AntiforgeryToken) {
        var url = "/AssignSubject/InsertAssignSubjectByAjax/";
        var AssignSubject = ({ CourseID: courseID, BatchID: batchID, SubjectAllocationID: subjectID });
        var data = JSON.stringify({ __RequestVerificationToken: AntiforgeryToken, AssignSubject: AssignSubject });
        AppUtil.MakeAjaxCall(url, "POST", data, AssignSubjectManager.InsertAssignSubjectSuccess, AssignSubjectManager.InsertAssignSubjectFailed);
    },
    InsertAssignSubjectSuccess: function (data) {
        
        //alert("success");
        if (data.information) {
            var s = JSON.parse(data.information);
            var infoSubjectAllocation = JSON.parse(data.resultSubjectAllocation);
            console.log("now : " + s.AssignSubjectID);
            $('#tblAssignSubject tbody').append('<tr><td style="padding:0px"> <input id="item_AssignSubjectID" type="hidden" value=' + s.AssignSubjectID + '> </td><td style="padding:0px"> <input id="item_CourseID" type="hidden" value=' + s.SubjectAllocation.CourseID + '> </td><td style="padding:0px"> <input id="item_BatchID" type="hidden" value=' + s.SubjectAllocation.BatchID + '> </td><td style="padding:0px"> <input id="item_SubjectAllocationID" type="hidden" value=' + s.SubjectAllocationID + '> </td><td>' + s.SubjectAllocation.Course.CourseName + '</td><td>' + infoSubjectAllocation.SubjectAllocation.Batch.BatchName + '</td><td>' + s.SubjectAllocation.Subject.SubjectName + '</td><td>' + ' <div style="width: 30%; float: left"><button type="button" id="btnEdit" class="btn btn-success btn-block" style="width: 40px;"><span class="glyphicon glyphicon-ok"></span></button></div><div style="width: 30%; float: left"><button type="button" id="btnDelete" class="btn btn-danger btn-block" style="width: 40px;"><span class="glyphicon glyphicon-remove"></span></button></div>' + '</td></tr>');;
            $('#lstCourse').prop('selectedIndex', 0);
            $("#lstBatch").find("option").not(":first").remove();
            $("#lstSubject").find("option").not(":first").remove();
        }

        if (data.Exist == true) {
            alert("Sorry this subject already assigned for this batch.");
        }
        AssignSubjectManager.GetAllAssignSubjectAfterSave();
        // $("#tblAssignSubject tbody").append('<tr><td>' + s.Course.CourseName + '</td><td>' + s.Batch.BatchName + '</td><td>' + s.SubjectAllocation.Subject.SubjectName + '</td><td>' + "sss" + '</td></tr>');
    },
    InsertAssignSubjectFailed: function (data) {
        alert("Fail");
        console.log(data);
    },

    DeleteAssignSubject: function (DeleteAssignSubjectID) {
        // var AntiforgeryToken = $('input[name="__RequestVerificationToken"]').val();
        var url = "/AssignSubject/DeleteAssignSubjectID/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ AssignSubjectID: DeleteAssignSubjectID, __RequestVerificationToken: AntiForgeryToken });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AssignSubjectManager.DeleteAssignSubjectSuccess, AssignSubjectManager.DeleteAssignSubjectError);
    },
    DeleteAssignSubjectSuccess: function (data) {
        //   alert("Success");
        console.log(data);
        
        if (data.InfoNotFound == true) {
            alert("Sorry No Assign SUbject found.");
        }

        if (data.success == true) {
            alert("Assign Subject Removed Success.");

            $("#tblAssignSubject tbody>tr").each(function () {
                var id = $(this).closest("tr").find("td:eq(0) input").val();
                console.log("id: " + id);
                if (id == data.DeleteAssignSubjectID) {
                    var index = $(this).index();
                    $('#tblAssignSubject tbody>tr:eq(' + index + ')').remove();
                }
            });

        }
        else if (data.success == false) {
            alert("Assign Subject Removed Failed.");
        }

    },
    DeleteAssignSubjectError: function (data) {
        alert("failed");
        console.log(data);
    },

    GetAssignSubjectByAssignSubjectIDForUpdate: function (updateAssignSubjectID) {
        var antiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var url = "/AssignSubject/GetAssignSubjectByID/";
        var data = ({ __RequestVerificationToken: antiForgeryToken, AssignSubjectID: updateAssignSubjectID });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AssignSubjectManager.GetAssignSubjectByAssignSubjectIDForUpdateSuccess, AssignSubjectManager.GetAssignSubjectByAssignSubjectIDForUpdateError);

    },
    GetAssignSubjectByAssignSubjectIDForUpdateSuccess: function (data) {
        
        //   alert("Success");
        var returnData = JSON.parse(data.lstSubjectAllocation);

        $("#lstCourse").find("option").not(":first").remove();
        $("#lstBatch").find("option").not(":first").remove();
        $("#lstSubject").find("option").not(":first").remove();

        $.each(returnData, function (index, item) {
            //
            //   alert(item);
            var i = (item);
            console.log("item: " + (i));

            var avialableCourse = false;
            $('#lstCourse option').each(function () {
                //    
                var val = $(this).attr('value');
                if (val == item.CourseID) {
                    avialableCourse = true;
                }
            });
            if (avialableCourse == false) {
                $("#lstCourse").append($("<option></option>").val(item.CourseID).text(item.Course.CourseName));
            }

            var avialableBatch = false;
            $('#lstBatch option').each(function () {
                //    
                var val = $(this).attr('value');
                if (val == item.BatchID) {
                    avialableBatch = true;
                }
            });
            if (avialableBatch == false) {
                $("#lstBatch").append($("<option></option>").val(item.BatchID).text(item.Batch.BatchName));
            }

            var avialableSubject = false;
            $('#lstSubject option').each(function () {
                //      
                var val = $(this).attr('value');
                if (val == item.SubjectID) {
                    avialableSubject = true;
                }
            });
            if (avialableSubject == false) {
                
                $("#lstSubject").append($("<option></option>").val(item.SubjectAllocationID).text(item.Subject.SubjectName));
            }


            //});
        });

        

        if (updateAssignCourseID) {
            $("#lstCourse").val(updateAssignCourseID);
        }
        if (updateAssignBatchID) {
            $("#lstBatch").val(updateAssignBatchID);
        }
        if (updateAssignSubjectAllocationID) {
            $("#lstSubject").val(updateAssignSubjectAllocationID);
        }
    },
    GetAssignSubjectByAssignSubjectIDForUpdateError: function (data) {
        
        updateMode = false;
        alert("fail");
        console.log(data);
    },

    UpdateAssignSubject: function (courseID, batchID, subjectID, updateAssignCourseID, updateAssignBatchID, updateAssignSubjectAllocationID, AntiforgeryToken) {
        
        var url = "/AssignSubject/UpdateAssignSubject/";
        var newAssignSubject = ({ SubjectAllocationID: subjectID });
        var oldAssignSubject = ({ SubjectAllocationID: updateAssignSubjectAllocationID });
        var data = JSON.stringify({ oldAssignSubject: oldAssignSubject, newAssignSubject: newAssignSubject });
        AppUtil.MakeAjaxCall(url, "POST", data, AssignSubjectManager.UpdateAssignSubjectSuccess, AssignSubjectManager.UpdateAssignSubjectError);

    },
    UpdateAssignSubjectSuccess: function (data) {
        
        if (data.Exist == true) {
            alert("Sorry this subject already assigned for this batch.");
        }
        if (data.UpdateStasus == true) {
            alert("Update Successfully.")
            var parseInfo = JSON.parse(data.AssignSubject);
            $("#tblAssignSubject tbody>tr").each(function () {
                
                var assignSubjectIDFromTable = $(this).find("td:eq(0) input").val();
                if (assignSubjectIDFromTable == parseInfo.AssignSubjectID) {
                    var rowIndex = $(this).index();
                    console.log(rowIndex);

                    $('#tblAssignSubject tbody>tr:eq(' + rowIndex + ')').find("td:eq(0) input").val(parseInfo.AssignSubjectID);
                    $('#tblAssignSubject tbody>tr:eq(' + rowIndex + ')').find("td:eq(1) input").val(parseInfo.SubjectAllocation.CourseID);
                    $('#tblAssignSubject tbody>tr:eq(' + rowIndex + ')').find("td:eq(2) input").val(parseInfo.SubjectAllocation.BatchID);
                    $('#tblAssignSubject tbody>tr:eq(' + rowIndex + ')').find("td:eq(3) input").val(parseInfo.SubjectAllocationID);
                    $('#tblAssignSubject tbody>tr:eq(' + rowIndex + ')').find("td:eq(4)").text(parseInfo.SubjectAllocation.Course.CourseName);
                    $('#tblAssignSubject tbody>tr:eq(' + rowIndex + ')').find("td:eq(5)").text(parseInfo.SubjectAllocation.Batch.BatchName);
                    $('#tblAssignSubject tbody>tr:eq(' + rowIndex + ')').find("td:eq(6)").text(parseInfo.SubjectAllocation.Subject.SubjectName);
                }
            });
            updateMode = false
        }
        AssignSubjectManager.GetAllCourseList();
        AssignSubjectManager.GetAllAssignSubjectAfterSave();

    },
    UpdateAssignSubjectError: function (data) {
        alert("fail");
    },

    GetAllCourseList: function () {
        
        var url = "/AssignSubject/GetAllCourseByAjax/";
        var anti = $("input[name='__RequestVerificationToken']").val();
        var data = ({ __RequestVerificationToken: anti });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AssignSubjectManager.GetAllCourseSuccess, AssignSubjectManager.GetAllCourseError);
    },
    GetAllCourseSuccess: function (data) {
        
        //  alert("Successs");
        var dta = JSON.parse(data.CourseList);

        console.log();
        $("#lstCourse").find("option").not(":first").remove();
        $("#lstBatch").find("option").not(":first").remove();
        $("#lstSubject").find("option").not(":first").remove();

        $.each(dta, function (index, item) {
            
            $("#lstCourse").append($("<option></option>").val(item.CourseID).text(item.Course.CourseName));

        });

        //console.log();
        //$("#lstcourse").find("option").not(":first").remove();
        //$("#lstbatch").find("option").not(":first").remove();
        //$("#lstsubject").find("option").not(":first").remove();
        //$.each(data.course, function (index, item) {
        //    
        //    $("#lstcourse").append($("<option></option>").val(item.courseid).text(item.course.coursename));
        //});
    },
    GetAllCourseError: function (data) {
        
        // alert("Successs");
    }
}