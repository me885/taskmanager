import { render, screen } from '@testing-library/react';
import App from '../App';

test('renders login page', () => {
  render(<App />);
  const loginButton = screen.getByText(/Login/i);
  expect(loginButton).toBeInTheDocument();
});
