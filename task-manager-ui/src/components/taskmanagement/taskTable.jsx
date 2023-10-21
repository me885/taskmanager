import { Container, Skeleton } from "@mui/material"
import { useEffect, useState } from "react"
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import EditTaskModal from './EditTaskModal';
import TableFilterPanel from "./tableFilterPanel";
import CompleteTaskButton from "./completeTaskButton"

const TaskTable = ({loading, setLoading}) => {
    const [tableRows, setTableRows] = useState([])
    const [selectedTask, setSelectedTask] = useState({})
    const [isOpen, setOpen] = useState(false);

    //filter state:
    const [isCompleteFilter, setIsCompleteFilter] = useState(false)
    const [prioritiesFilter, setPrioritiesFilter] = useState(["High", "Medium", "Low"])

    const handleEditTask = (task) => {
      setSelectedTask(task);
      setOpen(true);
    }

    useEffect(() => {
        async function fetchTasks()
        {
            await fetch("https://taskmanager-todo.azurewebsites.net/tasks?" + new URLSearchParams(
            {
              isComplete: isCompleteFilter,
              priorities: prioritiesFilter
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
                    setLoading(false)
                }
            })

        }
        fetchTasks();
    }, [isCompleteFilter, prioritiesFilter, loading, setLoading])

    

    return(
    <>
      <Container>
        <TableFilterPanel 
        isComplete={isCompleteFilter} 
        setIsComplete={setIsCompleteFilter}
        priorities={prioritiesFilter}
        setPriorities={setPrioritiesFilter}
        setTableLoading={setLoading}
        />
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
                  {loading ? <Skeleton/> : row.name}
                </TableCell>
                <TableCell align="right">
                  {loading ? <Skeleton/> : row.priority}
                </TableCell>
                <TableCell align="right">
                  {loading ? <Skeleton/> : row.deadline}
                </TableCell>
                <TableCell align="right">
                  {loading ? <Skeleton/> : row.description}
                </TableCell>
                <TableCell align="right">
                  {isCompleteFilter ? "date completed" : <CompleteTaskButton taskName={row.name} setTableLoading={setLoading}/>}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Container>
      <EditTaskModal isOpen={isOpen} setOpen={setOpen} task={selectedTask} setTableLoading={setLoading} />
      </>)
}

export default TaskTable