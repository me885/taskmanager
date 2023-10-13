import { useState } from 'react';
import Stack from '@mui/material/Stack';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Modal from '@mui/material/Modal';
import MenuItem from '@mui/material/MenuItem';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { DateField } from '@mui/x-date-pickers/DateField';


const CreateTaskButton = () => {
    
    const [isOpen, setOpen] = useState(false);
    const handleOpen = () => setOpen(true);
    const handleClose = () => setOpen(false);

    const handleTaskSubmit = async (event) => {
        setOpen(false);
        event.preventDefault();
        const formData = new FormData(event.currentTarget);

        const task = {
            name: formData.get("taskname"),
            description: formData.get("taskdescription"),
            deadline: formData.get("taskdeadline"),
            priority: formData.get("taskpriority")
        }

        const x = JSON.stringify(task)

        console.log(x)

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
                document.getElementById("login-form").reset()
                setOpen(false)
            }
        })
    }


    const style = {
        position: 'absolute',
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)',
        width: 500,
        bgcolor: 'lightgrey',
        border: '2px solid #000',
        boxShadow: 24,
        p: 4,
    };

    return (
        <>
        <LocalizationProvider dateAdapter={AdapterDayjs}>
        <Button
            id="logout-button"
            className="navbar-logout"
            size="small"
            variant="contained"
            onClick={handleOpen}
        >
            New Task
        </Button>
        <Modal
            open={isOpen}
            onClose={handleClose}
        >
            <Stack
            sx={style}
            id="create-task-form"
            component="form"
            onSubmit={handleTaskSubmit}
            spacing={2}
            noValidate
            autoComplete="off">
                <Typography id="create-task-title" variant="h5" component="h2" style={{margin: 10}}>
                    Create a new task
                </Typography>
                <TextField 
                    id="taskname" 
                    label="Task name" 
                    name="taskname"
                    style={{margin: 10}}
                    variant="outlined" />
                <TextField 
                    select
                    id="taskpriority" 
                    label="Task priority" 
                    name="taskpriority"
                    style={{margin: 10}}
                    variant="outlined">
                    <MenuItem key="high" value="high">
                        High
                    </MenuItem>
                    <MenuItem key="medium" value="medium">
                        Medium
                    </MenuItem>
                    <MenuItem key="low" value="low">
                        Low
                    </MenuItem>
                </TextField>
                <DateField 
                    label="Deadline" 
                    name="taskdeadline"
                    style={{margin: 10}}/>
                <TextField 
                    id="taskdescription" 
                    multiline
                    minRows={2}
                    maxRows={5}
                    label="Task decription" 
                    name="taskdescription"
                    style={{margin: 10}}
                    variant="outlined" />
                <Button 
                    type="submit"
                    variant="contained"
                    style={{margin: 10}}
                    color="primary">
                    Create
                </Button>
            </Stack>
        </Modal>
        </LocalizationProvider>
        </>
    )
}

export default CreateTaskButton;

