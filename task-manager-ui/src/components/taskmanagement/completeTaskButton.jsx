import { Button } from "@mui/material"

const CompleteTaskButton = ({taskName, setTableLoading}) => 
{
    const handleCompleteTask = async (name) => {
        await fetch(`https://taskmanager-todo.azurewebsites.net/task/complete/${name}`, {
            method: "POST",
            headers: {"Content-Type": "application/json", "Authorization": `Bearer ${localStorage.getItem("jwt")}`},
        })
        .catch(error => {
            console.log(error)
            console.log("failed");
        })
        .then(async (response) => {
            if(response.status === 200)
            {
                setTableLoading(true)
            }
        })
    }

    return(
        <Button 
            size="small" 
            variant="outlined" 
            onClick={(e) => { e.stopPropagation(); e.preventDefault(); handleCompleteTask(taskName);}}>
            Complete
        </Button>
    )
}

export default CompleteTaskButton