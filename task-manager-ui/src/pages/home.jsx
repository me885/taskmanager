import { useContext, useEffect, useState } from 'react';
import UserContext from '../UserContext';
import { useNavigate } from 'react-router';
import TaskTable from '../components/taskmanagement/taskTable';
import CreateTaskButton from '../components/taskmanagement/createTaskButton';
import { Container } from '@mui/material';

const Home = () => {
  const [isLoadingTable, setIsLoadingTable] = useState(true);
  const currentUser = useContext(UserContext)
  const navigate = useNavigate(); 

  useEffect(() => {
    if(!currentUser.isLoggedIn)
    {
      navigate("/signin")
    }
  })

  if(!currentUser.isLoggedIn)
  {
    return;
  }

  return (
    <Container>
      <div style={{textAlign:'right', padding:24}}>
        <CreateTaskButton setTableLoading={setIsLoadingTable} />
      </div>
      <TaskTable loading={isLoadingTable} setLoading={setIsLoadingTable} />
    </Container>
  );
}

export default Home;