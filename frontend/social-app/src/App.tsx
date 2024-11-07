import AuthProvider from "./AuthProvider";
import Routes from "./Routes";


function App() {
  return (
    <AuthProvider>
      <Routes />
    </AuthProvider>
  );
}

export default App;