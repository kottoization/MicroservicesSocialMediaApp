import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "./AuthProvider";
import axios from "axios";

interface UserRegister {
    userName: string;
    password: string;
    email: string;
    firstName: string;
    lastName: string;
}

const Register = () => {
    const { setToken } = useAuth();
    const navigate = useNavigate();

    // Local state for username and password
    const [user, setUser] = useState<UserRegister>({
        email: "",
        userName: "",
        password: "",
        firstName: "",
        lastName: "",
    });
    const [error, setError] = useState<string | null>(null);

    // Function to handle form submission
    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault(); // Prevent default form submission behavior

        try {
            // Send Register request to the backend
            const response = await axios.post("http://localhost:5000/User/Register", user);

            const token = response.data.token;
            setToken(token);

            navigate("/", { replace: true });
        } catch (err: any) {
            // Handle error and set error message
            setError("Invalid username or password");
        }
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setUser((prev) => ({ ...prev, [name]: value }));
    }

    return (
        <div>
            <h2>Register Page</h2>
            <form onSubmit={handleRegister}>
                <div>
                    <label htmlFor="username">Username:</label>
                    <input
                        type="text"
                        name="userName"
                        value={user.userName}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="password">Password:</label>
                    <input
                        type="password"
                        name="password"
                        value={user.password}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="email">email:</label>
                    <input
                        type="email"
                        name="email"
                        value={user.email}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="firstName">firstName:</label>
                    <input
                        type="firstName"
                        name="firstName"
                        value={user.firstName}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="lastName">lastName:</label>
                    <input
                        type="lastName"
                        name="lastName"
                        value={user.lastName}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                {error && <p style={{ color: "red" }}>{error}</p>}
                <button type="submit">Login</button>
            </form>
        </div>
    );
};

export default Register;
