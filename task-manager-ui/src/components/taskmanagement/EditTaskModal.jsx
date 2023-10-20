import { LocalizationProvider } from "@mui/x-date-pickers"
import EditTaskForm from "./EditTaskForm"
import { Modal } from "@mui/material"
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs"
import dayjs from "dayjs"

const EditTaskModal = ({isOpen, setOpen, task}) => 
{
    const handleTaskEdit = async (event) =>
    {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);

        const newTask = {
            name: formData.get("taskname"),
            description: formData.get("taskdescription"),
            deadline: dayjs(formData.get("taskdeadline")),
            priority: formData.get("taskpriority")
        }

        await fetch(`https://taskmanager-todo.azurewebsites.net/task/${task.name}`, {
            method: "PUT",
            headers: {"Content-Type": "application/json", "Authorization": `Bearer ${localStorage.getItem("jwt")}`},
            body: JSON.stringify(newTask),
        })
        .catch(error => {
            console.log(error)
            console.log("failed");
        })
        .then(async (response) => {
            if(response.status === 200)
            {
                setOpen(false)
                window.location.reload();
            }
        })
    }

    return(
        <LocalizationProvider dateAdapter={AdapterDayjs}>
        <Modal
            open={isOpen}
            onClose={() => (setOpen(false))}
        >
            <EditTaskForm handleTaskSubmit={handleTaskEdit} task={task} text={{headerText: "Update Task", buttonText: "Update"}}/>
        </Modal>
        </LocalizationProvider>
    )
}

export default EditTaskModal