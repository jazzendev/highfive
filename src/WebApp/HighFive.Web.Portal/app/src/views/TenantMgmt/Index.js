import React, { useState, useEffect } from "react";
import { makeStyles } from "@material-ui/core/styles";
import CircularProgress from "@material-ui/core/CircularProgress";
import Button from "@material-ui/core/Button";
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import TableContainer from "@material-ui/core/TableContainer";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import Paper from "@material-ui/core/Paper";
import Toolbar from "@material-ui/core/Toolbar";
import Pagination from "@material-ui/lab/Pagination";
import EditIcon from "@material-ui/icons/Edit";
import CreateIcon from "@material-ui/icons/Create";
import { authFetch } from "Utilities/authFetch";
import config from "config";

import Editor from "./Editor";
import { Index as ServiceEditor } from "./Service/Index";

const useStyles = makeStyles((theme) => ({
  root: {
    width: "100%",
  },
  paper: {
    width: "100%",
    marginBottom: theme.spacing(2),
  },
  table: {
    minWidth: 750,
  },
  pagination: {
    justifyContent: "center",
  },
  loading: {
    position: "static",
    top: 0,
  },
}));

function Index(props) {
  const [error, setError] = useState();
  const [isLoading, setIsLoading] = useState(true);
  const [items, setItems] = useState([]);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);

  const [isEditorOpen, setIsEditorOpen] = useState(false);
  const [isSvcEditorOpen, setIsSvcEditorOpen] = useState(false);
  const [selectedTenant, setSelectedTenant] = useState({ id: "" });

  const classes = useStyles();

  useEffect(() => {
    fetchData();
  }, []);

  const changePage = (num) => {
    setPage(num);
    fetchData(num);
  };

  const openEditor = (tenant, isService) => {
    setSelectedTenant(tenant);

    if (isService) {
      setIsSvcEditorOpen(true);
    } else {
      setIsEditorOpen(true);
    }
  };

  const closeEditor = () => {
    setIsEditorOpen(false);
    setIsSvcEditorOpen(false);
  };

  const refreshData = () => {
    fetchData(page);
  };

  const fetchData = (page = 1) => {
    const params = new URLSearchParams({
      size: 12,
      page: page - 1,
    });

    const url = `${config.apiUrl}/api/tenant?${params.toString()}`;

    authFetch(url, {
      method: "GET",
    })
      .then(
        (result) => {
          setItems(result.data.data);
          setTotal(Math.ceil(result.data.total / result.data.size));
          setError(null);
        },
        // Note: it's important to handle errors here
        // instead of a catch() block so that we don't swallow
        // exceptions from actual bugs in components.
        (error) => {
          setError(error);
        }
      )
      .then(() => setIsLoading(false))
      .catch((error) => setError(error));
  };

  if (isLoading) {
    return <CircularProgress></CircularProgress>;
  } else {
    return (
      <div className={classes.root}>
        {error ? (
          <div>Error: {error.message}</div>
        ) : (
          <Paper className={classes.paper}>
            <Toolbar>
              <Button
                variant="contained"
                color="primary"
                size="small"
                startIcon={<CreateIcon />}
                onClick={(e) => openEditor({ id: "" })}
              >
                New Item
              </Button>
            </Toolbar>
            <TableContainer>
              <Table className={classes.table} size="small">
                <TableHead>
                  <TableRow>
                    <TableCell>Name</TableCell>
                    <TableCell align="right">Domain</TableCell>
                    <TableCell align="right">Creation</TableCell>
                    <TableCell align="right">Operations</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {items.map((item) => (
                    <TableRow key={item.id}>
                      <TableCell component="th" scope="row">
                        {item.name}
                      </TableCell>
                      <TableCell align="right">{item.domain}</TableCell>
                      <TableCell align="right">{item.creationTime}</TableCell>
                      <TableCell align="right">
                        <Button
                          variant="text"
                          color="primary"
                          size="small"
                          startIcon={<EditIcon />}
                          m="2"
                          onClick={(e) => openEditor(item, true)}
                        >
                          Service
                        </Button>

                        <Button
                          variant="text"
                          color="primary"
                          size="small"
                          startIcon={<EditIcon />}
                          onClick={(e) => openEditor(item)}
                        >
                          Edit
                        </Button>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
            <Toolbar className={classes.pagination}>
              <Pagination
                count={total}
                color="primary"
                showFirstButton
                showLastButton
                onChange={(e, p) => changePage(p)}
              />
            </Toolbar>
          </Paper>
        )}
        <Editor
          open={isEditorOpen}
          id={selectedTenant.id}
          closeHandler={closeEditor}
          refreshHandler={refreshData}
        ></Editor>
        <ServiceEditor
          open={isSvcEditorOpen}
          tenant={selectedTenant}
          closeHandler={closeEditor}
        ></ServiceEditor>
      </div>
    );
  }
}

export default Index;
