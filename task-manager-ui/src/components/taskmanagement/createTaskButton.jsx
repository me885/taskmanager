import { useState } from 'react';
import Button from '@mui/material/Button';
import Modal from '@mui/material/Modal';
import EditTaskForm from './EditTaskForm'
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import dayjs from 'dayjs';


const CreateTaskButton = () => {
    
    const [isOpen, setOpen] = useState(false);
    const handleOpen = () => setOpen(true);
    const handleClose = () => setOpen(false);

    const handleTaskSubmit = async (event) => {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);

        const task = {
            name: formData.get("taskname"),
            description: formData.get("taskdescription"),
            deadline: dayjs(formData.get("taskdeadline")),
            priority: formData.get("taskpriority")
        }
        console.log(formData.get("taskdeadline"));

        await fetch("https://taskmanager-todo.azurewebsites.net/task", {
            method: "POST",
            headers: {"Content-Type": "application/json", "Authorization": `Bearer ${localStorage.getItem("jwt")}`},
            body: JSON.stringify(task),
        })
        .catch(error => {
            console.log(error)
            console.log("failed");
        })
        .then(async (response) => {
            if(response.status === 200)
            {
                setOpen(false)
                //window.location.reload();
            }
        })
    }


    

    return (
        <>
        <LocalizationProvider dateAdapter={AdapterDayjs}>
        <Button
            id="logout-button"
            className="navbar-logout"
            size="large"
            variant="contained"
            onClick={handleOpen}
        >
            New Task
        </Button>
        <Modal
            open={isOpen}
            onClose={handleClose}
        >
            <EditTaskForm handleTaskSubmit={handleTaskSubmit} task={{}} text={{headerText: "Create a new task", buttonText: "Create"}}/>
        </Modal>
        </LocalizationProvider>
        </>
    )
}

export default CreateTaskButton;

