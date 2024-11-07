import axios from "axios";
import { createContext, useContext, useEffect, useMemo, useState } from "react";


interface IAuthContext {
    token: string | null;
    setToken: (newToken: string) => void;
  }
const AuthContext = createContext<IAuthContext>({
    token: null,
    setToken: () => {}
  });

const AuthProvider = ({ children }: any) => {
    // State to hold the authentication token
    const [token, _setToken] = useState(localStorage.getItem("token"));

    // Function to set the authentication token
    const setToken = (newToken: string) => {
        _setToken(newToken);
    };

    useEffect(() => {
        if (token) {
            axios.defaults.headers.common["Authorization"] = "Bearer " + token;
            localStorage.setItem('token', token);
        } else {
            delete axios.defaults.headers.common["Authorization"];
            localStorage.removeItem('token')
        }
    }, [token]);

    // Memoized value of the authentication context
    const contextValue = useMemo(
        () => ({
            token,
            setToken,
        }),
        [token]
    );

    // Provide the authentication context to the children components
    return (
        <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
    );
};

export const useAuth = () => {
    return useContext(AuthContext);
};

export default AuthProvider;