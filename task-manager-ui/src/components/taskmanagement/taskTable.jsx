import { Container } from "@mui/material"
import { useEffect, useState } from "react"
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';

const TaskTable = () => {
    const [tableRows, setTableRows] = useState([])

    useEffect(() => {
        async function fetchTasks()
        {
            const taskList = await fetch("https://taskmanager-todo.azurewebsites.net/tasks", {
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
    }, [])

    

    return(
    <Container>
        <Table sx={{ minWidth: 650 , backgroundColor: "#eeeeee"}} aria-label="task-table">
        <TableHead>
          <TableRow>
            <TableCell>Task</TableCell>
            <TableCell align="right">Priority</TableCell>
            <TableCell align="right">Deadline</TableCell>
            <TableCell align="right">Description</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {tableRows.map((row) => (
            <TableRow
              key={row.name}
              sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
            >
              <TableCell component="th" scope="row">
                {row.name}
              </TableCell>
              <TableCell align="right">{row.priority}</TableCell>
              <TableCell align="right">{row.deadline}</TableCell>
              <TableCell align="right">{row.description}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </Container>)
}

export default TaskTable