const db = firebase.firestore();


const taskForm = document.getElementById('task-form');
const chatContainer = document.getElementById('chats-container');
const session_id = document.getElementById('session_id').value;
const user_id = document.getElementById('user_id').value;
const user_name = document.getElementById('user_name').value;

const saveTask = (username, message, userid, dateNow, timeStamp) =>
    db.doc(`Chats/${session_id}`).collection(`chatroom`).doc(dateNow).set({
        username,
        message,
        userid,
        timeStamp,
    });

const getTask = () => db.doc(`Chats/${session_id}`).collection(`chatroom`).get();

const onGetTasks = (callback) => db.doc(`Chats/${session_id}`).collection(`chatroom`).onSnapshot(callback);

const deleteTask = id => db.doc(`Chats/${session_id}`).collection(`chatroom`).doc(id).delete();

window.addEventListener('DOMContentLoaded', async (e) => {

    onGetTasks((querySnapshot) => {
        chatContainer.innerHTML = '';
        querySnapshot.forEach(doc => {

            const chat = doc.data();
            chat.id = doc.id;

            console.log(chat);

            if (chat.userid == user_id) {
                chatContainer.innerHTML += `<div class="balon1 p-2 m-0 position-relative" data-is="${chat.username} - ${chat.timeStamp}">
            <a class="float-right ">${chat.message} </a>
            <button class="float-right unstyled-button btn-delete" data-id="${chat.id}">x</button>
            </div>`;
            }
            else {
                chatContainer.innerHTML += `<div class="balon2 p-2 m-0 position-relative" data-is="${chat.username} - ${chat.timeStamp}">
            <a class="float-left sohbet2"> ${chat.message} </a>
            <button class="float-left unstyled-button btn-delete" data-id="${chat.id}">x</button>
            </div>`;
            }

            const btnDelete = document.querySelectorAll('.btn-delete');
            btnDelete.forEach(btn => {
                btn.addEventListener('click', async (e) => {
                    console.log(e.target)
                    await deleteTask(e.target.dataset.id);
                })
            })

            document.getElementById("chats-container").scrollTop = document.getElementById("chats-container").scrollHeight;
        })
    })
})

taskForm.addEventListener('submit', async (e) => {
    e.preventDefault();

    const message = taskForm['task-description'];
    const dateNow = Date.now().toString();
    const currentDate = new Date();
    const timeStamp = currentDate.toLocaleString('en-MY', { hour: 'numeric', minute: 'numeric', hour12: true })
    await saveTask(user_name, message.value, user_id, dateNow, timeStamp);

    getTask();
    taskForm.reset();
    message.focus();
})

