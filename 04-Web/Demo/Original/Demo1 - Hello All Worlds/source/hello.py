def application(environ, start_response):
    status = '200 OK'
    response_headers = [('Content-type', 'text/html')]
    start_response(status, response_headers)
    yield '<h1>Hello World (in Python!)</h1>\n'