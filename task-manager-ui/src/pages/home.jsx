import { useContext, useEffect } from 'react';
import UserContext from '../UserContext';
import { useNavigate } from 'react-router';
import TaskTable from '../components/taskmanagement/taskTable';
import CreateTaskButton from '../components/taskmanagement/createTaskButton';

const Home = () => {
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
    <><CreateTaskButton /><TaskTable /></>
  );
}

export default Home;