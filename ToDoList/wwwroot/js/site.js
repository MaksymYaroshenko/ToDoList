const taskInput = document.getElementById('new-task-input');
const taskBtn = document.getElementById('new-task-btn');
const taskList = document.getElementsByClassName("task-list-li");

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

for (i = 0; i < taskList.length; i++) {
    var closeSpan = document.createElement("SPAN");
    var closeSpanText = document.createTextNode("\u00D7");
    closeSpan.className = "close";
    closeSpan.appendChild(closeSpanText);
    taskList[i].appendChild(closeSpan);

    var importantSpan = document.createElement("SPAN");
    var importantSpantext = document.createTextNode("\u2606");
    var importantSpanTooltip = document.createElement("SPAN");
    importantSpanTooltip.className = "tooltipText";
    importantSpanTooltip.textContent = "Mark task as important.";
    importantSpan.className = "important";
    importantSpan.appendChild(importantSpanTooltip);
    importantSpan.appendChild(importantSpantext);
    taskList[i].appendChild(importantSpan);
}

var close = document.getElementsByClassName("close");
for (i = 0; i < close.length; i++) {
    close[i].onclick = function () {
        var task = new Object();
        task.id = this.parentElement.id;
        $.ajax({
            url: '/Home/DeleteTask',
            type: 'DELETE',
            data: task,
            success: function (data, textStatus, xhr) {
                location.reload();
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in "Delete" Operation');
            }
        });
    }
}

for (i = 0; i < taskList.length; i++) {
    if (taskList[i].value === 1) {
        taskList[i].classList.toggle('checked');
    }
    taskList[i].addEventListener('click', function (ev) {
        if (ev.target.tagName === 'LI') {
            var task = new Object();
            task.id = this.id;
            $.ajax({
                url: '/Home/CompleteTask',
                type: 'PUT',
                data: task,
                success: function (data, textStatus, xhr) {
                    location.reload();
                },
                error: function (xhr, textStatus, errorThrown) {
                    console.log('Error in "IsDone" Operation');
                }
            });
        }
    }, false);
}

var important = document.getElementsByClassName("important");
for (i = 0; i < important.length; i++) {
    if (taskList[i].tabIndex === 1) {
        important[i].textContent = "\u2605";
    }
    important[i].onclick = function () {
        var task = new Object();
        task.id = this.parentElement.id;
        $.ajax({
            url: '/Home/MakeImportant',
            type: 'PUT',
            data: task,
            success: function (data, textStatus, xhr) {
                location.reload();
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in "IsImportant" Operation');
            }
        });
    }
}