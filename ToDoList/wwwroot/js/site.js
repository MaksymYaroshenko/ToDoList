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
