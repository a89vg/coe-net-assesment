import { Login } from "../Login";

export function HomePage() {

  return (
    <div className="row">
      <div className="col">
        <h2>Welcome</h2>
        <Login />
      </div>
    </div>
  );
}
