import { Select, MenuItem, FormControl, InputLabel, Checkbox, Box, Chip } from "@mui/material"

const TableFilterPanel = ({isComplete, setIsComplete, priorities, setPriorities, setTableLoading}) =>
{
    return(
        <div style={{paddingBottom: 15, display:"flex", flexDirection: "row"}}>
             <FormControl size="large" style={{paddingRight: 10, minWidth: 200}}>
                <InputLabel id="priority-select-label">Priorities</InputLabel>
                <Select 
                id="priority-select"
                labelId="priority-select-label"
                label="Task"
                multiple
                value={priorities}
                onChange={(e) => 
                {
                    setPriorities(e.target.value)
                    setTableLoading(true)
                }}
                renderValue={(selected) => (
                    <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5 }}>
                      {selected.map((value) => (
                        <Chip key={value} label={value} />
                      ))}
                    </Box>
                  )}
                >
                    <MenuItem key="High" value="High">
                        <Checkbox checked={priorities.indexOf("High") > -1} />
                        High
                    </MenuItem>
                    <MenuItem key="Medium" value="Medium">
                        <Checkbox checked={priorities.indexOf("Medium") > -1} />
                        Medium
                    </MenuItem>
                    <MenuItem key="Low" value="Low">
                        <Checkbox checked={priorities.indexOf("Low") > -1} />
                        Low
                    </MenuItem>
                </Select>
            </FormControl>
             <FormControl size="large" >
                <InputLabel id="is-complete-select-label">Show</InputLabel>
                <Select 
                id="is-complete-select"
                labelId="is-complete-select-label"
                label="Task"
                value={isComplete}
                onChange={(e) => 
                {
                    setIsComplete(e.target.value)
                    setTableLoading(true)
                }}
                >
                    <MenuItem value={false}>Outstanding</MenuItem>
                    <MenuItem value={true}>Completed</MenuItem>
                </Select>
            </FormControl>
        </div>
    )
}

export default TableFilterPanel