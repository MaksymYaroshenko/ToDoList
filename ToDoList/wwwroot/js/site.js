const taskInput = document.getElementById('new-task-input');
const taskBtn = document.getElementById('new-task-btn');

function add_new_task_click() {
    document.getElementById('add-icon').hidden = true;
    document.getElementById('new-task').style.display = '';
    taskBtn.disabled = true;
    event.preventDefault();
}

taskInput.addEventListener('input', function () {
    if (taskInput.value.length > 0) {
        taskBtn.disabled = false;
    } else {
        taskBtn.disabled = true;
    }
});

var myNodelist = document.getElementsByClassName("task-list-li");
for (i = 0; i < myNodelist.length; i++) {
    var span = document.createElement("SPAN");
    var txt = document.createTextNode("\u00D7");
    span.className = "close";
    span.appendChild(txt);
    myNodelist[i].appendChild(span);
}

var close = document.getElementsByClassName("close");
for (i = 0; i < close.length; i++) {
    close[i].onclick = function () {
        var task = new Object();
        task.id = this.parentElement.id;
        $.ajax({
            url: 'https://localhost:44315/Home/DeleteTask',
            type: 'DELETE',
            data: task,
            success: function (data, textStatus, xhr) {
                location.reload();
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation');
            }
        });
    }
}