import { Select, MenuItem, FormControl, InputLabel } from "@mui/material"

const TableFilterPanel = ({isComplete, setIsComplete}) =>
{
    return(
        <div style={{padding: 10, paddingBottom: 15}}>
             <FormControl size="medium">
                <InputLabel id="is-complete-select-label">Show</InputLabel>
                <Select 
                id="is-complete-select"
                labelId="is-complete-select-label"
                label="Task"
                value={isComplete}
                onChange={(e) => setIsComplete(e.target.value)}
                >
                    <MenuItem value={false}>Outstanding</MenuItem>
                    <MenuItem value={true}>Completed</MenuItem>
                </Select>
            </FormControl>
        </div>
    )
}

export default TableFilterPanel