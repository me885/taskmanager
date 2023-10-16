import { Button, MenuItem, Stack, TextField, Typography } from "@mui/material"
import { DateTimeField } from "@mui/x-date-pickers";
import dayjs from "dayjs";

const EditTaskForm = ({handleTaskSubmit, task, text: {headerText, buttonText}}) => {
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

    return(
        <Stack
        sx={style}
        id="create-task-form"
        component="form"
        onSubmit={handleTaskSubmit}
        spacing={2}
        noValidate
        autoComplete="off">
            <Typography id="create-task-title" variant="h5" component="h2" style={{margin: 10}}>
                {headerText}
            </Typography>
            <TextField 
                id="taskname" 
                label="Task name" 
                name="taskname"
                defaultValue={task.name}
                style={{margin: 10}}
                variant="outlined" />
            <TextField 
                select
                id="taskpriority" 
                label="Task priority" 
                name="taskpriority"
                defaultValue={task.priority !== undefined ? task.priority.toLowerCase() : undefined}
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
            <DateTimeField 
                label="Deadline" 
                name="taskdeadline"
                defaultValue={dayjs(task.deadline)}
                views={['year', 'month', 'day', 'hours', 'minutes']}
                style={{margin: 10}}/>
            <TextField 
                id="taskdescription" 
                multiline
                minRows={2}
                maxRows={5}
                label="Task decription" 
                name="taskdescription"
                defaultValue={task.description}
                style={{margin: 10}}
                variant="outlined" />
            <Button
                type="submit"
                variant="contained"
                style={{margin: 10}}
                color="primary">
                {buttonText}
            </Button>
        </Stack>
    )
}

export default EditTaskForm