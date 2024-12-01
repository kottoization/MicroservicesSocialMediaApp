import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "./AuthProvider";
import axios from "axios";

const Login = () => {
    const { setToken } = useAuth();
    const navigate = useNavigate();

    // Local state for username and password
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<string | null>(null);

    // Function to handle form submission
    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault(); // Prevent default form submission behavior

        try {
            // Send login request to the backend
              const response = await axios.post("http://localhost:5000/User/login", { 
                username, 
                password 
              });

            const token = response.data.token;
            setToken(token);

            navigate("/", { replace: true });
        } catch (err: any) {
            // Handle error and set error message
            setError("Invalid username or password");
        }
    };

    return (
        <div>
            <h2>Login Page</h2>
            <form onSubmit={handleLogin}>
                <div>
                    <label htmlFor="username">Username:</label>
                    <input
                        type="text"
                        id="username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="password">Password:</label>
                    <input
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                {error && <p style={{ color: "red" }}>{error}</p>}
                <button type="submit">Login</button>
            </form>
        </div>
    );
};

export default Login;
