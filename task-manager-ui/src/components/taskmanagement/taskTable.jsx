import { Container } from "@mui/material"
import { useEffect, useState } from "react"
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import EditTaskModal from './EditTaskModal';
import TableFilterPanel from "./tableFilterPanel";
import CompleteTaskButton from "./completeTaskButton"

const TaskTable = () => {
    const [tableRows, setTableRows] = useState([])
    const [selectedTask, setSelectedTask] = useState({})
    const [isOpen, setOpen] = useState(false);


    const [isCompleteFilter, setIsCompleteFilter] = useState(false)


    const handleEditTask = (task) => {
      setSelectedTask(task);
      setOpen(true);
    }

    useEffect(() => {
        async function fetchTasks()
        {
            await fetch("https://taskmanager-todo.azurewebsites.net/tasks?" + new URLSearchParams(
            {
              isComplete: isCompleteFilter
            }), 
            {
                method: "GET",
                headers: {"Content-Type": "application/json", "Authorization": `Bearer ${localStorage.getItem("jwt")}`},
            })
            .catch(error => {
                console.log(error)
            })
            .then(async (response) => {
                if(response.status === 200)
                {
                    const taskList = await response.json();
                    setTableRows(taskList);
                }
            })

        }
        fetchTasks();
    }, [isCompleteFilter])

    

    return(
    <>
      <Container>
        <TableFilterPanel isComplete={isCompleteFilter} setIsComplete={setIsCompleteFilter}/>
        <Table sx={{ minWidth: 650, backgroundColor: "#eeeeee" }} aria-label="task-table">
          <TableHead style={{ backgroundColor: "#d0d0d0" }}>
            <TableRow>
              <TableCell><b>Task</b></TableCell>
              <TableCell align="right"><b>Priority</b></TableCell>
              <TableCell align="right"><b>Deadline</b></TableCell>
              <TableCell align="right"><b>Description</b></TableCell>
              <TableCell align="right"><b></b></TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {tableRows.map((row) => (
              <TableRow
                key={row.name}
                sx={{ '&:last-child td, &:last-child th': { border: 0 }, cursor: "pointer" }}
                onClick={() => { handleEditTask(row);}}
                hover
              >
                <TableCell component="th" scope="row">
                  {row.name}
                </TableCell>
                <TableCell align="right">{row.priority}</TableCell>
                <TableCell align="right">{row.deadline}</TableCell>
                <TableCell align="right">{row.description}</TableCell>
                <TableCell align="right">
                  <CompleteTaskButton taskName={row.name}/>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Container>
      <EditTaskModal isOpen={isOpen} setOpen={setOpen} task={selectedTask} />
      </>)
}

export default TaskTable